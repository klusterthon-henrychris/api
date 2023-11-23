namespace Kluster.Shared.DTOs.Responses.User;

public record UserResponse(string FirstName,
    string LastName,
    string EmailAddress,
    string Address, string Role);