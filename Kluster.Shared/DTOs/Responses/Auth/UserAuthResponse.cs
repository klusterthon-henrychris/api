namespace Kluster.Shared.DTOs.Responses.Auth;

public record UserAuthResponse(string Id,
    string Role,
    string AccessToken);