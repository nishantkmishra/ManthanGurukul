using AutoMapper;
using ManthanGurukul.Application.Interfaces;
using ManthanGurukul.Application.Services.Interfaces;
using ManthanGurukul.Application.Users;
using ManthanGurukul.Domain.Entities;
using ManthanGurukul.Shared.DTOs;

namespace ManthanGurukul.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAsyncRepository<User> _repository;
        private readonly IMapper _mapper;

        public UserService(IAsyncRepository<User> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<UserDto> SignInAsync(SignInUserRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }
            else
            {

                var user = await _repository.GetFirstOrDefaultAsync(x => x.MobileNo == request.MobileNo);
                if (user == null)
                {
                    throw new Exception($"User with mobile no {request.MobileNo} not found.");
                }
                else
                {
                    if (!Shared.Helpers.PasswordHasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
                    {
                        throw new Exception("Invalid Credentials");
                    }
                }
                return _mapper.Map<UserDto>(user);
            }
        }
    }
}
