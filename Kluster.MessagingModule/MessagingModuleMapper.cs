using Kluster.Shared.DTOs.Requests.Invoices;
using Kluster.Shared.MessagingContracts.Events.Invoices;

namespace Kluster.Messaging;

public static class MessagingModuleMapper
{
    public static SendInitialInvoiceEmailRequest ToSendInitialInvoiceEmailRequest(
        InvoiceCreatedEvent invoiceCreatedEvent)
    {
        return new SendInitialInvoiceEmailRequest(
            invoiceCreatedEvent.FirstName,
            invoiceCreatedEvent.LastName,
            invoiceCreatedEvent.EmailAddress,
            invoiceCreatedEvent.DueDate,
            invoiceCreatedEvent.BusinessName,
            invoiceCreatedEvent.InvoiceId);
    }
}