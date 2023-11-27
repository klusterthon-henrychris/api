using Kluster.Shared.MessagingContracts.Events.Invoices;

namespace Kluster.NotificationModule.Services.Contracts;

public interface IReminderService
{
    Task SendInvoiceReminder(InvoiceCreatedEvent invoiceCreatedEvent);
}