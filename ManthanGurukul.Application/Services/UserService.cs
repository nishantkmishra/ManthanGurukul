using AutoMapper;
using ManthanGurukul.Application.Exceptions;
using ManthanGurukul.Application.Interfaces;
using ManthanGurukul.Application.Services.Interfaces;
using ManthanGurukul.Application.Users;
using ManthanGurukul.Domain.Entities;
using ManthanGurukul.Shared.DTOs;
using ManthanGurukul.Shared.Helpers;

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
            var user = await _repository.GetFirstOrDefaultAsync(x => x.MobileNo == request.MobileNo && x.IsActive == true);
            if (user == null)
            {
                throw new NotFoundException($"User with mobile no {request.MobileNo} not found or account inactive.");
            }
            else
            {
                if (!PasswordHasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
                {
                    throw new BadRequestException("Invalid Credentials");
                }
            }
            return _mapper.Map<UserDto>(user);
        }
    }
}
