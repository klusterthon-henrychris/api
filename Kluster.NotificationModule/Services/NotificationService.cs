﻿using System.Globalization;
using System.Web;
using ErrorOr;
using Kluster.NotificationModule.Models;
using Kluster.NotificationModule.ServiceErrors;
using Kluster.NotificationModule.Services.Contracts;
using Kluster.Shared.Configuration;
using Kluster.Shared.DTOs.Requests.Invoices;
using Kluster.Shared.DTOs.Requests.Notification;
using Kluster.Shared.MessagingContracts.Commands.Notification;
using Kluster.Shared.SharedContracts.NotificationModule;
using Microsoft.Extensions.Options;

namespace Kluster.NotificationModule.Services;

public class NotificationService(IMailService mailService, IOptionsSnapshot<MailSettings> options)
    : INotificationService
{
    private readonly MailSettings _mailSettings = options.Value;

    public async Task<bool> SendOtpMail(SendOtpEmailRequest request)
    {
        var emailTemplate = await mailService.LoadTemplateFromBlob(nameof(SendOtpMail));
        List<string> to = [request.EmailAddress];
        emailTemplate = emailTemplate
            .Replace("{FirstName}", request.FirstName)
            .Replace("{LastName}", request.LastName)
            .Replace("{Token}", HttpUtility.UrlEncode(request.Otp))
            // todo: remove userId from SendOtpEmailRequest
            .Replace("{BaseUrl}", _mailSettings.BaseWebsiteUrl);

        return await mailService.SendAsync(new MailData
        {
            Attachments = null,
            Body = emailTemplate,
            Subject = "Verify your email address!",
            To = to
        }, new CancellationToken());
    }

    public async Task<bool> SendWelcomeMail(string emailAddress, string firstName, string lastName)
    {
        var emailTemplate = await mailService.LoadTemplateFromBlob(nameof(SendWelcomeMail));
        List<string> to = [emailAddress];
        emailTemplate = emailTemplate
            .Replace("{FirstName}", firstName)
            .Replace("{LastName}", lastName);

        return await mailService.SendAsync(new MailData
        {
            Attachments = null,
            Body = emailTemplate,
            Subject = "Welcome To SimpleBiz",
            To = to
        }, new CancellationToken());
    }

    public async Task<ErrorOr<Success>> SendForgotPasswordMail(SendForgotPasswordEmailCommand request)
    {
        var emailTemplate = await mailService.LoadTemplateFromBlob(nameof(SendWelcomeMail));
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

    public async Task<ErrorOr<Success>> SendInitialInvoiceMail(SendInitialInvoiceEmailRequest request)
    {
        var emailTemplate = await mailService.LoadTemplateFromBlob(nameof(SendInitialInvoiceMail));
        List<string> to = [request.EmailAddress];
        emailTemplate = emailTemplate
            .Replace("{FirstName}", request.FirstName)
            .Replace("{LastName}", request.LastName)
            .Replace("{DueDate}", request.DueDate.ToString("dd:MM:yyyy HH:mm"))
            .Replace("{InvoiceNo}", request.InvoiceNo)
            .Replace("{ReplyToEmail}", _mailSettings.From)
            .Replace("{BaseUrl}", _mailSettings.BaseWebsiteUrl);

        var success = await mailService.SendAsync(new MailData
        {
            Attachments = null,
            Body = emailTemplate,
            Subject = $"Invoice from {request.BusinessName}. Due on {request.DueDate:dd:MM:yyyy HH:mm}.",
            To = to
        }, new CancellationToken());

        return success ? Result.Success : Errors.Notification.InitialInvoiceEmailFailed;
    }

    public async Task<ErrorOr<Success>> SendInvoiceReminderMail(SendInvoiceReminderRequest request)
    {
        var emailTemplate = await mailService.LoadTemplateFromBlob(nameof(SendInvoiceReminderMail));
        List<string> to = [request.EmailAddress];
        emailTemplate = emailTemplate
            .Replace("{FirstName}", request.FirstName)
            .Replace("{DueDate}", request.DueDate.ToString("dd:MM:yyyy HH:mm"))
            .Replace("{IssuedDate}", request.IssuedDate.ToString("dd:MM:yyyy HH:mm"))
            .Replace("{InvoiceNo}", request.InvoiceNo)
            .Replace("{Amount}", request.Amount.ToString(CultureInfo.CurrentCulture))
            .Replace("{ReplyToEmail}", _mailSettings.From);
        
        var success = await mailService.SendAsync(new MailData
        {
            Attachments = null,
            Body = emailTemplate,
            Subject =
                $"Invoice Reminder From {request.BusinessName}. Payment {request.InvoiceStatus} - ₦{request.Amount}.",
            To = to
        }, new CancellationToken());

        return success ? Result.Success : Errors.Notification.InitialReminderEmailFailed;
    }
}