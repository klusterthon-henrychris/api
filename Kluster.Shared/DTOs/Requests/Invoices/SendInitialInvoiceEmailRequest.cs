namespace Kluster.Shared.DTOs.Requests.Invoices;

public record SendInitialInvoiceEmailRequest(
    string FirstName,
    string LastName,
    string EmailAddress,
    DateTime DueDate,
    string BusinessName,
    string InvoiceNo);