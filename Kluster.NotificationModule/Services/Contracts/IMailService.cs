using Kluster.NotificationModule.Models;

namespace Kluster.NotificationModule.Services.Contracts;

public interface IMailService
{
    Task<bool> SendAsync(MailData mailData, CancellationToken ct);
    Task<bool> SendWithAttachmentsAsync(MailDataWithAttachments mailData, CancellationToken ct);
    public string LoadTemplate(string emailTemplate);
}