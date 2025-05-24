using ManthanGurukul.Application.UseCases.Auth;

namespace ManthanGurukul.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync(AuthRequest request);
    }
}
