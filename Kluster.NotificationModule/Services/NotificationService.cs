using System.Web;
using ErrorOr;
using Kluster.NotificationModule.Models;
using Kluster.NotificationModule.ServiceErrors;
using Kluster.NotificationModule.Services.Contracts;
using Kluster.Shared.DTOs.Requests.Notification;
using Kluster.Shared.MessagingContracts.Commands.Notification;
using Kluster.Shared.SharedContracts.NotificationModule;

namespace Kluster.NotificationModule.Services;

public class NotificationService(IMailService mailService) : INotificationService
{
    public Task<bool> SendOtpEmail(SendOtpEmailRequest request)
    {
        var emailTemplate = mailService.LoadTemplate(nameof(SendOtpEmail));
        List<string> to = [request.EmailAddress];
        emailTemplate = emailTemplate
            .Replace("{FirstName}", request.FirstName)
            .Replace("{LastName}", request.LastName)
            .Replace("{Token}", HttpUtility.UrlEncode(request.Otp))
            .Replace("{UserId}", request.UserId);

        return mailService.SendAsync(new MailData
        {
            Attachments = null,
            Body = emailTemplate,
            Subject = "Verify your email address!",
            To = to
        }, new CancellationToken());
    }

    public Task<bool> SendWelcomeMail(string emailAddress, string firstName, string lastName)
    {
        var emailTemplate = mailService.LoadTemplate(nameof(SendWelcomeMail));
        List<string> to = [emailAddress];
        emailTemplate = emailTemplate
            .Replace("{FirstName}", firstName)
            .Replace("{LastName}", lastName);

        return mailService.SendAsync(new MailData
        {
            Attachments = null,
            Body = emailTemplate,
            Subject = "Welcome To SimpleBiz",
            To = to
        }, new CancellationToken());
    }

    public async Task<ErrorOr<Success>> SendForgotPasswordMail(SendForgotPasswordEmailCommand request)
    {
        var emailTemplate = mailService.LoadTemplate(nameof(SendWelcomeMail));
        List<string> to = [request.EmailAddress];
        emailTemplate = emailTemplate
            .Replace("{FirstName}", request.FirstName)
            .Replace("{LastName}", request.LastName)
            .Replace("{Token}", request.Token);

        var success = await mailService.SendAsync(new MailData
        {
            Attachments = null,
            Body = emailTemplate,
            Subject = "Reset Your Password.",
            To = to
        }, new CancellationToken());

        return success ? Result.Success : Errors.Notification.ForgotPasswordEmailFailed;
    }
}