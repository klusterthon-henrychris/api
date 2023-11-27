using Kluster.Shared.MessagingContracts.Events.Invoices;
using Kluster.Shared.SharedContracts.NotificationModule;
using MassTransit;
using InvalidOperationException = System.InvalidOperationException;

namespace Kluster.Messaging.Consumers.Notifications;

/// <summary>
/// Responds when an invoice is created to notify the Client of its existence
/// </summary>
/// <param name="notificationService"></param>
public class SendInitialInvoiceEmailInvoiceCreatedEventConsumer(
    INotificationService notificationService,
    ILogger<SendInitialInvoiceEmailInvoiceCreatedEventConsumer> logger) : IConsumer<InvoiceCreatedEvent>
{
    public async Task Consume(ConsumeContext<InvoiceCreatedEvent> context)
    {
        logger.LogInformation(
            "Invoice {0} has been created. Sending Initial Invoice Email...", context.Message.InvoiceId);
        var result =
            await notificationService.SendInitialInvoiceMail(
                MessagingModuleMapper.ToSendInitialInvoiceEmailRequest(context.Message));
        if (result.IsError)
        {
            logger.LogError(result.FirstError.Description);
            throw new InvalidOperationException(result.FirstError.Code);
        }

        logger.LogInformation("Successfully sent Initial email for invoice {0}.", context.Message.InvoiceId);
    }
}