using System.Text;
using Kluster.NotificationModule.Models;
using Kluster.NotificationModule.Services.Contracts;
using Kluster.Shared.Configuration;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Kluster.NotificationModule.Services;

public class MailService(IOptionsSnapshot<MailSettings> settings, ILogger<MailService> logger) : IMailService
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
            mail.From.Add(new MailboxAddress(_settings.DisplayName, _settings.From));
            mail.Sender = new MailboxAddress(_settings.DisplayName, _settings.From);

            // Receiver
            if (mailData.To != null)
            {
                foreach (var mailAddress in mailData.To)
                {
                    mail.To.Add(MailboxAddress.Parse(mailAddress));
                }
            }

            #endregion

            #region Content

            // Add Content to Mime Message
            var body = new BodyBuilder();
            mail.Subject = mailData.Subject;
            body.HtmlBody = mailData.Body;
            mail.Body = body.ToMessageBody();

            #endregion

            // Check if we got any attachments and add the to the builder for our message
            // todo: extract method
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

            await Send(mail, ct);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.ToString());
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
    
    /// <inheritdoc />
    public string LoadTemplate(string pathToTemplate)
    {
        var baseDir = Directory.GetCurrentDirectory();

        var templateDir = Path.Combine(baseDir, "Templates");
        var templatePath = Path.Combine(templateDir, $"{pathToTemplate}.html");

        using var fileStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var streamReader = new StreamReader(fileStream, Encoding.Default);

        var mailTemplate = streamReader.ReadToEnd();
        streamReader.Close();

        return mailTemplate;
    }
}