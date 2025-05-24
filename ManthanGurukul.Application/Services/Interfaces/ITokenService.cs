﻿using System.Security.Claims;

namespace ManthanGurukul.Application.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();
    }
}
