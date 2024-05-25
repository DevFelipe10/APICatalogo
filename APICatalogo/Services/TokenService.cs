using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace APICatalogo.Services;

public class TokenService : ITokenService
{
    public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config)
    {
        //Claims = lista de informações do usuário    

        var key = _config.GetSection("JWT").GetValue<string>("SecretKey") ??
                    throw new InvalidOperationException("Invalid secret key");

        var privateKey = Encoding.UTF8.GetBytes(key);

        // Criar as credenciais para assinar o token
        var siginigCredetials = new SigningCredentials(new SymmetricSecurityKey(privateKey),
                                    SecurityAlgorithms.HmacSha256Signature);

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_config.GetSection("JWT")
                                                .GetValue<double>("TokenValidityInMinutes")),
            Audience = _config.GetSection("JWT").GetValue<string>("ValidAudience"),
            Issuer = _config.GetSection("JWT").GetValue<string>("ValidIssuer"),
            SigningCredentials = siginigCredetials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateJwtSecurityToken(tokenDescription);

        return token;
    }

    public string GenerateRefreshToken()
    {
        var secureRandomBytes = new byte[128];

        using var randomNumberGenerator = RandomNumberGenerator.Create();

        randomNumberGenerator.GetBytes(secureRandomBytes);

        var refreshToken = Convert.ToBase64String(secureRandomBytes);

        return refreshToken;
    }

    public ClaimsPrincipal GetPricipalFromExpiredToken(string token, IConfiguration _config)
    {
        var secretKey = _config["JWT:SecretKey"] ?? throw new InvalidOperationException("Invalid Key");

        var tokenValidationsParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(secretKey)),
            ValidateLifetime = false,
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, tokenValidationsParameters,
                                                  out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                          !jwtSecurityToken.Header.Alg.Equals(
                              SecurityAlgorithms.HmacSha256,
                              StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}
