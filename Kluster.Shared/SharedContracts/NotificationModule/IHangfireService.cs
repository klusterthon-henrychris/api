using Kluster.Shared.MessagingContracts.Events.Invoices;

namespace Kluster.Shared.SharedContracts.NotificationModule;

public interface IHangfireService
{
    Task ScheduleInvoiceReminders(InvoiceCreatedEvent invoiceEvent);
}