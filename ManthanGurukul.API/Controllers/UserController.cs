using ManthanGurukul.Application.Services.Interfaces;
using ManthanGurukul.Application.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManthanGurukul.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInUserRequest request)
        {
            try
            {
                var result = await _userService.SignInAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
