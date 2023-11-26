namespace Kluster.Shared.DTOs.Requests.User;

public record ResetPasswordRequest(string Token, string EmailAddress, string Password);