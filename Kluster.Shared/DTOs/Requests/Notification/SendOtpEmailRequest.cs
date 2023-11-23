namespace Kluster.Shared.DTOs.Requests.Notification;

public record SendOtpEmailRequest(string UserId, string Otp, string FirstName, string LastName, string EmailAddress);