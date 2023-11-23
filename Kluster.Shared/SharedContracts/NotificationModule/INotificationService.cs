using Kluster.Shared.DTOs.Requests.Notification;

namespace Kluster.Shared.SharedContracts.NotificationModule;

public interface INotificationService
{
    Task SendOtpEmail(SendOtpEmailRequest sendOtpEmailRequest);
}