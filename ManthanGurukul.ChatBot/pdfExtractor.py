import re
import pdfplumber
import nltk
from nltk.tokenize import sent_tokenize
from sentence_transformers import SentenceTransformer, util
import numpy as np
import pytesseract
from pdf2image import convert_from_path
import logging
import sys
import io
from difflib import get_close_matches
import requests

sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')

logging.getLogger("pdfminer").setLevel(logging.ERROR)
logging.getLogger("pdfplumber").setLevel(logging.ERROR)

# Path to your local GGUF model (e.g., Llama 2, Mistral, etc.)

class PDFChatbot:
    def __init__(self):
        nltk.download('punkt')
        self.pdf_text = ""
        self.sentences = []
        self.chunks = []
        self.chunk_embeddings = None
        self.sentence_embeddings = None
        self.model = SentenceTransformer('all-MiniLM-L6-v2')
        self.pdf_path = None
        self.entity_name = None

    def load_pdf(self, pdf_path, chunk_size=6, overlap=3):
        self.pdf_path = pdf_path
        try:
            with pdfplumber.open(pdf_path) as pdf:
                self.pdf_text = ""
                for page in pdf.pages:
                    text = page.extract_text()
                    if text:
                        self.pdf_text += text + " "
            if not self.pdf_text.strip():
                print("No text could be extracted from the PDF.")
                return False
            self.sentences = sent_tokenize(self.pdf_text)
            self.chunks = self.chunk_pdf_text()
            self.chunk_embeddings = self.model.encode(self.chunks, convert_to_tensor=True)
            self.sentence_embeddings = self.model.encode(self.sentences, convert_to_tensor=True)
            self.entity_name = self.extract_entity_name()
            return True
        except Exception as e:
            print(f"Error loading PDF: {str(e)}")
            return False

    def extract_entity_name(self):
        try:
            with pdfplumber.open(self.pdf_path) as pdf:
                first_page = pdf.pages[0]
                text = first_page.extract_text()
                if text:
                    for line in text.splitlines():
                        line = line.strip()
                        if line and not re.match(r'^\d{1,2}/\d{1,2}/\d{2,4}', line) and len(line.split()) > 1:
                            return line
        except Exception:
            pass
        return "the institution"

    def chunk_pdf_text(self):
        paragraphs = [p.strip() for p in re.split(r'\n\s*\n', self.pdf_text) if p.strip()]
        if not paragraphs:
            paragraphs = self.sentences
        return paragraphs

    def extract_field(self, field_name):
        lines = self.pdf_text.splitlines()
        all_fields = [line.strip().split(':')[0] for line in lines if ':' in line]
        close_matches = get_close_matches(field_name, all_fields, n=1, cutoff=0.7)
        if close_matches:
            field_name = close_matches[0]
        try:
            with pdfplumber.open(self.pdf_path) as pdf:
                for page in pdf.pages:
                    tables = page.extract_tables()
                    for table in tables:
                        for row in table:
                            if row and len(row) >= 2:
                                for i, cell in enumerate(row):
                                    if cell and field_name.lower() in str(cell).strip().lower():
                                        if i + 1 < len(row) and row[i + 1]:
                                            return str(row[i + 1]).strip()
        except Exception:
            pass

        field_pattern = re.compile(r'^[A-Za-z ]+\s*[:\-•*]')
        bullet_pattern = re.compile(r'^[\u2022\-\*\•]\s*')
        value_lines = []

        for i, line in enumerate(lines):
            line_clean = line.strip()
            pattern = re.compile(
                rf"^{re.escape(field_name)}\s*[:\-•*]?\s*(.*)", re.IGNORECASE
            )
            match = pattern.match(line_clean)
            if match:
                value = match.group(1).strip()
                if value:
                    value_lines = [value]
                else:
                    value_lines = []
                for j in range(i + 1, len(lines)):
                    next_line = lines[j].strip()
                    if not next_line or field_pattern.match(next_line) or bullet_pattern.match(next_line):
                        break
                    value_lines.append(next_line)
                if value_lines:
                    return " ".join(value_lines).strip()
            bullet_line_pattern = re.compile(
                rf"^[\u2022\-\*\•]\s*{re.escape(field_name)}\s*[:\-•*]?\s*(.*)", re.IGNORECASE
            )
            bullet_match = bullet_line_pattern.match(line_clean)
            if bullet_match:
                value = bullet_match.group(1).strip()
                if value:
                    value_lines = [value]
                else:
                    value_lines = []
                for j in range(i + 1, len(lines)):
                    next_line = lines[j].strip()
                    if not next_line or field_pattern.match(next_line) or bullet_pattern.match(next_line):
                        break
                    value_lines.append(next_line)
                if value_lines:
                    return " ".join(value_lines).strip()
        try:
            pages = convert_from_path(self.pdf_path)
            for page_img in pages:
                ocr_text = pytesseract.image_to_string(page_img)
                ocr_lines = ocr_text.splitlines()
                for i, line in enumerate(ocr_lines):
                    line_clean = line.strip()
                    pattern = re.compile(
                        rf"^{re.escape(field_name)}\s*[:\-•*]?\s*(.*)", re.IGNORECASE
                    )
                    match = pattern.match(line_clean)
                    if match:
                        value = match.group(1).strip()
                        if value:
                            value_lines = [value]
                        else:
                            value_lines = []
                        for j in range(i + 1, len(ocr_lines)):
                            next_line = ocr_lines[j].strip()
                            if not next_line or field_pattern.match(next_line) or bullet_pattern.match(next_line):
                                break
                            value_lines.append(next_line)
                        if value_lines:
                            return " ".join(value_lines).strip()
                    bullet_line_pattern = re.compile(
                        rf"^[\u2022\-\*\•]\s*{re.escape(field_name)}\s*[:\-•*]?\s*(.*)", re.IGNORECASE
                    )
                    bullet_match = bullet_line_pattern.match(line_clean)
                    if bullet_match:
                        value = bullet_match.group(1).strip()
                        if value:
                            value_lines = [value]
                        else:
                            value_lines = []
                        for j in range(i + 1, len(ocr_lines)):
                            next_line = ocr_lines[j].strip()
                            if not next_line or field_pattern.match(next_line) or bullet_pattern.match(next_line):
                                break
                            value_lines.append(next_line)
                        if value_lines:
                            return " ".join(value_lines).strip()
        except Exception:
            pass

        return None

    def call_ollama(self, prompt, model="llama2", max_tokens=256):
        url = "http://localhost:11434/api/generate"
        payload = {
            "model": model,
            "prompt": prompt,
            "stream": False,
            "options": {
                "num_predict": max_tokens,
                "temperature": 0.2
            }
        }
        response = requests.post(url, json=payload)
        response.raise_for_status()
        return response.json()["response"].strip()

    def get_response(self, question, sentence_threshold=0.30):
        q = question.strip().rstrip("?").strip(":")
        field_name = q
        m = re.search(r"(?:what is|show|display|give me|find)\s+(.+)", q, re.IGNORECASE)
        if m:
            field_name = m.group(1).strip()
        value = self.extract_field(field_name)
        if value:
            return value

        # Use chunk-based semantic search for context-rich answers
        if hasattr(self, 'chunks') and hasattr(self, 'chunk_embeddings'):
            question_embedding = self.model.encode(question, convert_to_tensor=True)
            chunk_scores = util.pytorch_cos_sim(question_embedding, self.chunk_embeddings)[0].cpu().numpy()
            best_idx = int(np.argmax(chunk_scores))
            best_chunk = self.chunks[best_idx]

            prompt = (
                f"You are an intelligent assistant. "
                f"Given the following context from a PDF document, answer the user's question as accurately and concisely as possible.\n\n"
                f"Context:\n{best_chunk}\n\n"
                f"Question: {question}\n"
                f"Answer:"
            )

            try:
                answer = self.call_ollama(prompt)
                return answer
            except Exception as e:
                return f"Error calling Ollama: {e}"

        # Fallback to sentence-based search
        if self.sentences and self.sentence_embeddings is not None:
            question_embedding = self.model.encode(question, convert_to_tensor=True)
            sent_scores = util.pytorch_cos_sim(question_embedding, self.sentence_embeddings)[0].cpu().numpy()
            best_idx = int(np.argmax(sent_scores))
            return self.sentences[best_idx]

        return "Sorry, I could not find an answer to your question."

def cli():
    import argparse
    parser = argparse.ArgumentParser()
    parser.add_argument('--pdf', required=True, help='Path to PDF file')
    parser.add_argument('--question', required=True, help='Question to ask')
    args = parser.parse_args()

    chatbot = PDFChatbot()
    if not chatbot.load_pdf(args.pdf):
        print("Failed to load PDF.", file=sys.stderr)
        sys.exit(1)
    response = chatbot.get_response(args.question)
    print(response)

if __name__ == "__main__":
    cli()