using Kluster.Shared.MessagingContracts.Events.Notification;
using Kluster.Shared.SharedContracts.NotificationModule;
using MassTransit;

namespace Kluster.Messaging.Consumers.Notifications;

public class WelcomeUserEventConsumer(
    INotificationService notificationService,
    ILogger<WelcomeUserEventConsumer> logger) : IConsumer<WelcomeUserEvent>
{
    public async Task Consume(ConsumeContext<WelcomeUserEvent> context)
    {
        logger.LogInformation("Received WELCOME EMAIL request for: {0}.", context.Message.EmailAddress);
        var success = await notificationService.SendWelcomeMail(context.Message.EmailAddress, context.Message.FirstName,
            context.Message.LastName);
        if (!success)
        {
            throw new Exception(string.Format("{0} failed for email: {1}", nameof(WelcomeUserEvent), context.Message.EmailAddress));
        }

        // todo: don't log email address
        logger.LogInformation("Sent WELCOME EMAIL to: {0}.", context.Message.EmailAddress);
    }
}