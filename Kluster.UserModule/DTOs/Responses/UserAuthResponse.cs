namespace Kluster.UserModule.DTOs.Responses;

public record UserAuthResponse(string Id,
    string FirstName,
    string LastName,
    string EmailAddress,
    string Role,
    string AccessToken);