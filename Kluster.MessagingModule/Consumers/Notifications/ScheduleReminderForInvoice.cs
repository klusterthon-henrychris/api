using Kluster.Shared.MessagingContracts.Events.Invoices;
using Kluster.Shared.SharedContracts.NotificationModule;
using MassTransit;

namespace Kluster.Messaging.Consumers.Notifications;

public class ScheduleReminderForInvoice(ILogger<ScheduleReminderForInvoice> logger, IHangfireService hangfireService) : IConsumer<InvoiceCreatedEvent>
{
    public Task Consume(ConsumeContext<InvoiceCreatedEvent> context)
    {
        logger.LogInformation("Invoice {0} has been created. Scheduling a reminder...", context.Message.InvoiceId);
        return hangfireService.ScheduleInvoiceReminders(context.Message);
    }
}