using Kluster.NotificationModule.Models;
using Kluster.NotificationModule.Services.Contracts;
using Kluster.Shared.Configuration;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Kluster.NotificationModule.Services;

public class MailService(IOptionsSnapshot<MailSettings> settings) : IMailService
{
    private readonly MailSettings _settings = settings.Value;

    public async Task<bool> SendAsync(MailData mailData, CancellationToken ct = default)
    {
        try
        {
            // Initialize a new instance of the MimeKit.MimeMessage class
            var mail = new MimeMessage();

            #region Sender / Receiver

            // Sender
            mail.From.Add(new MailboxAddress(_settings.DisplayName, mailData.From ?? _settings.From));
            mail.Sender = new MailboxAddress(mailData.DisplayName ?? _settings.DisplayName,
                mailData.From ?? _settings.From);

            // Receiver
            foreach (var mailAddress in mailData.To)
                mail.To.Add(MailboxAddress.Parse(mailAddress));

            // Set Reply to if specified in mail data
            if (!string.IsNullOrEmpty(mailData.ReplyTo))
            {
                mail.ReplyTo.Add(new MailboxAddress(mailData.ReplyToName, mailData.ReplyTo));
            }

            // BCC
            // Check if a BCC was supplied in the request
            if (mailData.Bcc != null)
            {
                // Get only addresses where value is not null or with whitespace. x = value of address
                foreach (var mailAddress in mailData.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
                    mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
            }

            // CC
            // Check if a CC address was supplied in the request
            if (mailData.Cc != null)
            {
                foreach (var mailAddress in mailData.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
                    mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
            }

            #endregion

            #region Content

            // Add Content to Mime Message
            var body = new BodyBuilder();
            mail.Subject = mailData.Subject;
            body.HtmlBody = mailData.Body;
            mail.Body = body.ToMessageBody();

            #endregion

            await Send(mail, ct);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> SendWithAttachmentsAsync(MailDataWithAttachments mailData, CancellationToken ct)
    {
        try
        {
            // Initialize a new instance of the MimeKit.MimeMessage class
            var mail = new MimeMessage();

            #region Sender / Receiver

            // Sender
            mail.From.Add(new MailboxAddress(_settings.DisplayName, mailData.From ?? _settings.From));
            mail.Sender = new MailboxAddress(mailData.DisplayName ?? _settings.DisplayName,
                mailData.From ?? _settings.From);

            // Receiver
            if (mailData.To != null)
                foreach (var mailAddress in mailData.To)
                {
                    mail.To.Add(MailboxAddress.Parse(mailAddress));
                }

            // Set Reply to if specified in mail data
            if (!string.IsNullOrEmpty(mailData.ReplyTo))
            {
                mail.ReplyTo.Add(new MailboxAddress(mailData.ReplyToName, mailData.ReplyTo));
            }

            // BCC
            // Check if a BCC was supplied in the request
            if (mailData.Bcc != null)
            {
                // Get only addresses where value is not null or with whitespace. x = value of address
                foreach (var mailAddress in mailData.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
                    mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
            }

            // CC
            // Check if a CC address was supplied in the request
            if (mailData.Cc != null)
            {
                foreach (var mailAddress in mailData.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
                    mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
            }

            #endregion

            #region Content

            // Add Content to Mime Message
            var body = new BodyBuilder();
            mail.Subject = mailData.Subject;
            body.HtmlBody = mailData.Body;
            mail.Body = body.ToMessageBody();

            // Check if we got any attachments and add the to the builder for our message
            if (mailData.Attachments != null)
            {
                foreach (var attachment in mailData.Attachments)
                {
                    // Check if length of the file in bytes is larger than 0
                    if (attachment.Length <= 0)
                    {
                        continue;
                    }

                    // Create a new memory stream and attach attachment to mail body
                    byte[] attachmentFileByteArray;
                    using (var memoryStream = new MemoryStream())
                    {
                        // Copy the attachment to the stream
                        await attachment.CopyToAsync(memoryStream, ct);
                        attachmentFileByteArray = memoryStream.ToArray();
                    }

                    // Add the attachment from the byte array
                    body.Attachments.Add(attachment.FileName, attachmentFileByteArray,
                        ContentType.Parse(attachment.ContentType));
                }
            }

            #endregion

            #region Send Mail

            await Send(mail, ct);

            // log success
            return true;

            #endregion
        }
        catch (Exception)
        {
            // log error
            return false;
        }
    }

    private async Task Send(MimeMessage mail, CancellationToken ct)
    {
        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(_settings.Host, _settings.Port, GetSecureSocketOptions(), ct);
            await smtp.AuthenticateAsync(_settings.UserName, _settings.Password, ct);
            await smtp.SendAsync(mail, ct);
        }
        finally
        {
            await smtp.DisconnectAsync(true, ct);
        }
    }

    private SecureSocketOptions GetSecureSocketOptions()
    {
        return _settings.UseSsl ? SecureSocketOptions.SslOnConnect :
               _settings.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.None;
    }
}