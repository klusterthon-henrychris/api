using Kluster.Shared.DTOs.Requests.Notification;
using Kluster.Shared.SharedContracts.NotificationModule;

namespace Kluster.NotificationModule.Services;

public class NotificationService(ILogger<NotificationService> logger) : INotificationService
{
    public Task SendOtpEmail(SendOtpEmailRequest sendOtpEmailRequest)
    {
        logger.LogInformation(
            $"Otp sent to: {sendOtpEmailRequest.FirstName + " " + sendOtpEmailRequest.LastName}." +
            $"\n OTP: {sendOtpEmailRequest.Otp}");
        return Task.CompletedTask;
    }
}