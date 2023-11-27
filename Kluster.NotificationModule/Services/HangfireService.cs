using Hangfire;
using Kluster.NotificationModule.Services.Contracts;
using Kluster.Shared.MessagingContracts.Events.Invoices;
using Kluster.Shared.SharedContracts.NotificationModule;

namespace Kluster.NotificationModule.Services;

public class HangfireService(
    IReminderService reminderService,
    ILogger<HangfireService> logger) : IHangfireService
{
    public async Task ScheduleInvoiceReminders(InvoiceCreatedEvent invoiceEvent)
    {
        // todo: need invoice status and issuedDate
        var currentTime = DateTime.UtcNow;
        var timeUntilScheduled = invoiceEvent.DueDate - currentTime;
        var scheduledDate = invoiceEvent.DueDate;

        if (timeUntilScheduled.TotalSeconds <= 0)
        {
            logger.LogInformation("Invoice {0} was scheduled too early. No reminder sent.", invoiceEvent.InvoiceId);
            // Scheduled date is in the past, do not schedule anything.
            return;
        }

        if (timeUntilScheduled.TotalMinutes <= 60)
        {
            // schedule in 2 minutes -> for demo.
            logger.LogInformation("A demo reminder has been scheduled for invoice {0}.", invoiceEvent.InvoiceId);
            BackgroundJob.Schedule(() => reminderService.SendInvoiceReminder(invoiceEvent),
                TimeSpan.FromMinutes(2));
            return;
        }

        // schedule for invoice due date.
        BackgroundJob.Schedule(() => reminderService.SendInvoiceReminder(invoiceEvent), scheduledDate);
        logger.LogInformation("A reminder has been scheduled for the invoice {0}'s due date, {1}.",
            invoiceEvent.InvoiceId, invoiceEvent.DueDate);
        
        // check
        RecurringJob.AddOrUpdate(invoiceEvent.InvoiceId,
            () => reminderService.SendInvoiceReminder(invoiceEvent), Cron.Weekly);
    }
}