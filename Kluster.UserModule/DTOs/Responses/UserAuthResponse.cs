namespace Kluster.UserModule.DTOs.Responses;

public record UserAuthResponse(string Id,
    string Role,
    string AccessToken);