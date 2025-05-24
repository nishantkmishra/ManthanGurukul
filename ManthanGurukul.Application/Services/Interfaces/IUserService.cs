using ManthanGurukul.Application.Users;
using ManthanGurukul.Shared.DTOs;

namespace ManthanGurukul.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> SignInAsync(SignInUserRequest request);
    }
}
