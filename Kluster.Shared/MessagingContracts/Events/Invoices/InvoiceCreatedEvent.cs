namespace Kluster.Shared.MessagingContracts.Events.Invoices;

public record InvoiceCreatedEvent(
    string BusinessId,
    string ClientId,
    string InvoiceId,
    decimal Amount,
    string FirstName,
    string LastName,
    string EmailAddress,
    DateTime DueDate,
    string BusinessName);