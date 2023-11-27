using Kluster.Shared.DTOs.Requests.Invoices;
using Kluster.Shared.SharedContracts.NotificationModule;
using MassTransit;

namespace Kluster.Messaging.Consumers.Notifications;

public class SendInvoiceReminderConsumer(
    INotificationService notificationService,
    ILogger<SendInvoiceReminderConsumer> logger) : IConsumer<SendInvoiceReminderRequest>
{
    public async Task Consume(ConsumeContext<SendInvoiceReminderRequest> context)
    {
        var result = await notificationService.SendInvoiceReminderMail(context.Message);
        if (result.IsError)
        {
            logger.LogError("Failed to send invoice reminder. Error: {0}", result.FirstError);
        }
    }
}