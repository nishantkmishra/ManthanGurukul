using ManthanGurukul.Application.UseCases.ChatBot;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ManthanGurukul.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatBotController : ControllerBase
    {
        // Consider moving these to configuration for production use
        private const string PdfFilePath = @"C:\Users\nisha\Downloads\TestReport_NISHANT MISHRA_256030000811_4294e6ca-1ac5-4130-aca5-dc4fa072dfa5.pdf";
        private const string ScriptPath = @"C:\Projects\ManthanGurukul\ManthanGurukul.ChatBot\pdfExtractor.py";

        private readonly ILogger<ChatBotController> _logger;

        public ChatBotController(ILogger<ChatBotController> logger)
        {
            _logger = logger;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] AskRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Question))
                return BadRequest("Question is required.");

            if (!System.IO.File.Exists(PdfFilePath))
                return NotFound("PDF file not found on server.");

            if (!System.IO.File.Exists(ScriptPath))
                return NotFound("Python script not found on server.");

            var psi = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"\"{ScriptPath}\" --pdf \"{PdfFilePath}\" --question \"{request.Question}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            psi.Environment["PYTHONIOENCODING"] = "utf-8";

            try
            {
                using var process = Process.Start(psi);
                if (process == null)
                {
                    _logger.LogError("Failed to start Python process.");
                    return StatusCode(500, "Failed to start Python process.");
                }

                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    _logger.LogError("Python script error: {Error}", error);
                    return BadRequest(new { error = error.Trim() });
                }

                return Ok(output.Trim());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while running Python script.");
                return StatusCode(500, "Internal server error while processing your request.");
            }
        }
    }
}