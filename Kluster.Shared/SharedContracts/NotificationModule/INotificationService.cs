using ErrorOr;
using Kluster.Shared.DTOs.Requests.Invoices;
using Kluster.Shared.DTOs.Requests.Notification;
using Kluster.Shared.MessagingContracts.Commands.Notification;

namespace Kluster.Shared.SharedContracts.NotificationModule;

public interface INotificationService
{
    Task<bool> SendOtpMail(SendOtpEmailRequest id);
    Task<bool> SendWelcomeMail(string emailAddress, string firstName, string lastName);
    Task<ErrorOr<Success>> SendForgotPasswordMail(SendForgotPasswordEmailCommand request);
    Task<ErrorOr<Success>> SendInitialInvoiceMail(SendInitialInvoiceEmailRequest request);
    Task<ErrorOr<Success>> SendInvoiceReminderMail(SendInvoiceReminderRequest request);
}