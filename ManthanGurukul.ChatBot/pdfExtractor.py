import re
import pdfplumber
import nltk
from nltk.tokenize import sent_tokenize
from sentence_transformers import SentenceTransformer, util
import numpy as np
import pytesseract
from pdf2image import convert_from_path
import logging

logging.getLogger("pdfminer").setLevel(logging.ERROR)
logging.getLogger("pdfplumber").setLevel(logging.ERROR)

class PDFChatbot:
    def __init__(self):
        nltk.download('punkt')
        self.pdf_text = ""
        self.sentences = []
        self.sentence_embeddings = None
        self.model = SentenceTransformer('all-MiniLM-L6-v2')
        self.pdf_path = None

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
            self.sentence_embeddings = self.model.encode(self.sentences, convert_to_tensor=True)
            return True
        except Exception as e:
            print(f"Error loading PDF: {str(e)}")
            return False

    def extract_field(self, field_name):
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

        lines = self.pdf_text.splitlines()
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

    def get_response(self, question, sentence_threshold=0.30):
        q = question.strip().rstrip("?").strip(":")
        field_name = q
        m = re.search(r"(?:what is|when is|what are|what is my|when is my|tell me| tell me about|tell me about my|show|display|give me|find)\s+(.+)", q, re.IGNORECASE)
        if m:
            field_name = m.group(1).strip()
        value = self.extract_field(field_name)
        if value:
            return f'{field_name}: {value}'

        if not self.sentences or self.sentence_embeddings is None:
            return "No content loaded from the PDF."

        question_embedding = self.model.encode(question, convert_to_tensor=True)
        sent_scores = util.pytorch_cos_sim(question_embedding, self.sentence_embeddings)[0].cpu().numpy()
        best_idx = int(np.argmax(sent_scores))

        return self.sentences[best_idx]

def main():
    chatbot = PDFChatbot()
    pdf_path = r"C:\Users\nisha\Downloads\View Candidate Admit Card.pdf"
    if not chatbot.load_pdf(pdf_path):
        print("Failed to load PDF. Exiting...")
        return

    print("How may i help you today!")

    while True:
        user_input = input("\nYou: ")
        if user_input.lower() == 'quit':
            print("Goodbye!")
            break
        response = chatbot.get_response(user_input)
        print(f"Bot: {response}")

if __name__ == "__main__":
    main()