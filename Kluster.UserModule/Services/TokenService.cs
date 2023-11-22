using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Kluster.Shared.Configuration;
using Kluster.Shared.Constants;
using Kluster.UserModule.Services.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Kluster.UserModule.Services;

public class TokenService(IOptionsSnapshot<JwtSettings> options) : ITokenService
{
    private readonly JwtSettings _jwtSettings = options.Value;

    public string CreateUserJwt(string emailAddress, string userRole, string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey!));


        var claims = new List<Claim>
        {
            new(JwtClaims.Email, emailAddress),
            new(JwtClaims.UserId, userId),
            new(JwtClaims.Role, userRole)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _jwtSettings.Audience,
            Issuer = _jwtSettings.Issuer,
            Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_jwtSettings.TokenLifeTimeInHours)),
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}