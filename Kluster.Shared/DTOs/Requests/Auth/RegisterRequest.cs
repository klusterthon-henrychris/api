namespace Kluster.Shared.DTOs.Requests.Auth;

public record RegisterRequest(string FirstName, string LastName, string EmailAddress, string Address, string Password, string Role);