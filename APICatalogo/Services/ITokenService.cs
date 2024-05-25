﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APICatalogo.Services;

public interface ITokenService
{
    JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims,
                                         IConfiguration _config);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPricipalFromExpiredToken(string token,
                                                IConfiguration _config);
}
