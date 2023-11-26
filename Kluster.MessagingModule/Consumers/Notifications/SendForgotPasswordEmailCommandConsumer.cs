using Kluster.Shared.MessagingContracts.Commands.Notification;
using Kluster.Shared.SharedContracts.NotificationModule;
using MassTransit;

namespace Kluster.Messaging.Consumers.Notifications;

public class SendForgotPasswordEmailCommandConsumer(
    INotificationService notificationService,
    ILogger<SendForgotPasswordEmailCommandConsumer> logger)
    : IConsumer<SendForgotPasswordEmailCommand>
{
    public async Task Consume(ConsumeContext<SendForgotPasswordEmailCommand> context)
    {
        logger.LogInformation($"Received FORGOT PASSWORD email request for: {context.Message.EmailAddress}.");
        var result = await notificationService.SendForgotPasswordMail(context.Message);
        if (result.IsError)
        {
            logger.LogError(result.FirstError.Description);
            throw new InvalidOperationException(result.FirstError.Code);
        }
        logger.LogInformation($"Sent FORGOT PASSWORD email to: {context.Message.EmailAddress}.");
    }
}