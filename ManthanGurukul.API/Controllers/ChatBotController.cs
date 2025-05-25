using ManthanGurukul.Application.UseCases.ChatBot;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace ManthanGurukul.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatBotController : ControllerBase
    {
        private readonly ILogger<ChatBotController> _logger;

        public ChatBotController(ILogger<ChatBotController> logger)
        {
            _logger = logger;
        }

        private async Task<string> CallOllamaAsync(string prompt, string model = "deepseek-r1", int maxTokens = 256)
        {
            using var httpClient = new HttpClient();
            var url = "http://localhost:11434/api/chat";
            var payload = new
            {
                model = model,
                messages = new[]
                {
                    new { role = "system", content = "You are an intelligent assistant." },
                    new { role = "user", content = prompt }
                },
                stream = false,
                options = new
                {
                    num_predict = maxTokens,
                    temperature = 0.2
                }
            };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("message").GetProperty("content").GetString();
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] AskRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Question))
                return BadRequest("Question is required.");

            try
            {
                string answer = await CallOllamaAsync(request.Question);
                return Ok(answer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while calling Ollama.");
                return StatusCode(500, "Internal server error while processing your request.");
            }
        }
    }
}