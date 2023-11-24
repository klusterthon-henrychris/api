using Kluster.Shared.DTOs.Requests.Notification;

namespace Kluster.Shared.SharedContracts.NotificationModule;

public interface INotificationService
{
    Task<bool> SendOtpEmail(SendOtpEmailRequest Id);

    Task<bool> SendWelcomeMail(string emailAddress, string firstName, string lastName);
}