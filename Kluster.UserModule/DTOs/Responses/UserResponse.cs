namespace Kluster.UserModule.DTOs.Responses;

public record UserResponse(string FirstName,
    string LastName,
    string EmailAddress,
    string Address, string Role);