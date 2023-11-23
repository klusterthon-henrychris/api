namespace Kluster.Shared.DTOs.Requests.Notification;

public record SendOtpEmailRequest(string Otp, string FirstName, string LastName, string EmailAddress);