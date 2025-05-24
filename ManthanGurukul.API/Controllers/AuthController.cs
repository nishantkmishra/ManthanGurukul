using ManthanGurukul.Application.Services.Interfaces;
using ManthanGurukul.Application.UseCases.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManthanGurukul.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signIn")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            var response = await _authService.AuthenticateAsync(request);
            return Ok(response);
        }
    }
}
