namespace Kluster.UserModule.DTOs.Requests;

public record RegisterRequest(string FirstName, string LastName, string EmailAddress, string Address, string Password, string Role);