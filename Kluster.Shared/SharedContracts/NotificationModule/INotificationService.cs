using ErrorOr;
using Kluster.Shared.DTOs.Requests.Notification;
using Kluster.Shared.DTOs.Requests.User;
using Kluster.Shared.MessagingContracts.Commands.Notification;

namespace Kluster.Shared.SharedContracts.NotificationModule;

public interface INotificationService
{
    Task<bool> SendOtpEmail(SendOtpEmailRequest Id);

    Task<bool> SendWelcomeMail(string emailAddress, string firstName, string lastName);
    Task<ErrorOr<Success>> SendForgotPasswordMail(SendForgotPasswordEmailCommand request);
}