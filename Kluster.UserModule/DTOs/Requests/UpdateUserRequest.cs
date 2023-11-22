namespace Kluster.UserModule.DTOs.Requests;

public record UpdateUserRequest(string? FirstName, string? LastName, string? Address);