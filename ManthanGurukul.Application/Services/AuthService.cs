using AutoMapper;
using ManthanGurukul.Application.Exceptions;
using ManthanGurukul.Application.Interfaces;
using ManthanGurukul.Application.Services.Interfaces;
using ManthanGurukul.Application.UseCases.Auth;
using ManthanGurukul.Domain.Entities;
using ManthanGurukul.Shared.DTOs;
using ManthanGurukul.Shared.Helpers;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace ManthanGurukul.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IAsyncRepository<User> _asyncRepository;
        private readonly IAsyncRepository<RefreshToken> _refreshTokenRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(ITokenService tokenService, IAsyncRepository<User> asyncRepository, IAsyncRepository<RefreshToken> refreshTokenRepository, 
                           IMapper mapper, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _asyncRepository = asyncRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<AuthResponse> AuthenticateAsync(AuthRequest request)
        {
            var user = await _asyncRepository.GetFirstOrDefaultAsync(x => x.MobileNo == request.MobileNo && x.IsActive == true);
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

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.MobilePhone, user.MobileNo.ToString())
                };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(double.Parse(jwtSettings["RefreshTokenExpirationDays"])),
                IsRevoked = false,
                IsUsed = false
            };

            await _refreshTokenRepository.AddAsync(refreshTokenEntity);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = _mapper.Map<UserDto>(user)
            };
        }

        // Implement RefreshTokenAsync and RevokeTokenAsync similarly
    }

}
