using ManthanGurukul.Shared.DTOs;

namespace ManthanGurukul.Application.UseCases.Auth
{
    public class AuthResponse
    {
        public UserDto User { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
