namespace Kluster.Shared.DTOs.Requests.Invoices;

public record SendInvoiceReminderRequest(
    string InvoiceNo,
    string FirstName,
    decimal Amount,
    DateTime DueDate,
    DateTime IssuedDate,
    string ReplyTo,
    string EmailAddress,
    string InvoiceStatus,
    string BusinessName);