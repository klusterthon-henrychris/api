namespace Kluster.Shared.DTOs.Requests.Invoices;

public record SendInvoiceReminderRequest(
    string InvoiceNo,
    string FirstName,
    string LastName,
    decimal Amount,
    DateTime DueDate,
    DateTime IssuedDate,
    string EmailAddress,
    string InvoiceStatus,
    string BusinessName);