using Kluster.Shared;
using Kluster.Shared.SharedContracts.UserModule;

namespace Kluster.UserModule.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public string? UserId =>
        // Retrieve the user ID from the JWT token
        httpContextAccessor.HttpContext?.User.FindFirst(JwtClaims.UserId)?.Value;

    public string? Email =>
        // Retrieve the email from the JWT token
        httpContextAccessor.HttpContext?.User.FindFirst(JwtClaims.Email)?.Value;

    public string? Role =>
        // Retrieve the role from the JWT token
        httpContextAccessor.HttpContext?.User.FindFirst(JwtClaims.Role)?.Value;
}