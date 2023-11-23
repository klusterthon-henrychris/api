﻿using Kluster.Shared.DTOs.Requests.Notification;
using Kluster.Shared.MessagingContracts.Events.User;
using Kluster.Shared.SharedContracts.NotificationModule;
using Kluster.Shared.SharedContracts.UserModule;
using MassTransit;

namespace Kluster.Messaging.Consumers.Auth;

public class EmailOtpRequestedEventConsumer(
    IUserService userService,
    INotificationService notificationService,
    ILogger<EmailOtpRequestedEventConsumer> logger)
    : IConsumer<EmailOtpRequestedEvent>
{
    public async Task Consume(ConsumeContext<EmailOtpRequestedEvent> context)
    {
        var otp = await userService.GenerateOtpForEmail(context.Message.UserId);
        var success = await notificationService.SendOtpEmail(new SendOtpEmailRequest(context.Message.UserId, otp,
            context.Message.FirstName,
            context.Message.LastName,
            context.Message.EmailAddress));

        if (!success)
        {
            throw new Exception($"{nameof(EmailOtpRequestedEvent)} failed for user: {context.Message.UserId}");
        }

        logger.LogInformation($"Sent OTP email for: {context.Message.UserId}.");
    }
}