using System.Text;
using Azure.Storage.Blobs;
using Kluster.NotificationModule.Models;
using Kluster.NotificationModule.Services.Contracts;
using Kluster.Shared.Configuration;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Kluster.NotificationModule.Services;

public class MailService(
    IOptionsSnapshot<MailSettings> settings,
    IOptionsSnapshot<KeyVault> options,
    ILogger<MailService> logger) : IMailService
{
    private readonly MailSettings _mailSettings = settings.Value;
    private readonly KeyVault _keyVault = options.Value;
    private BlobClient _blobClient;

    public async Task<bool> SendAsync(MailData mailData, CancellationToken ct = default)
    {
        try
        {
            // Initialize a new instance of the MimeKit.MimeMessage class
            var mail = new MimeMessage();

            #region Sender / Receiver

            // Sender
            mail.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.From));
            mail.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.From);

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
            await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, GetSecureSocketOptions(), ct);
            await smtp.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password, ct);
            await smtp.SendAsync(mail, ct);
        }
        finally
        {
            await smtp.DisconnectAsync(true, ct);
        }
    }

    private SecureSocketOptions GetSecureSocketOptions()
    {
        return _mailSettings.UseSsl ? SecureSocketOptions.SslOnConnect :
            _mailSettings.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.None;
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

    public async Task<string> LoadTemplateFromBlob(string templateName)
    {
        _blobClient = new BlobClient(_keyVault.AZURE_STORAGE_CONNECTION_STRING, _keyVault.BLOB_CONTAINER_NAME,
            $"{templateName}.html");

        var blob = await _blobClient.DownloadContentAsync();
        return blob.Value.Content.ToString();
    }
}