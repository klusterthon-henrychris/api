namespace Kluster.Shared.DTOs.Requests.Invoices;

public record CreateInvoiceRequest(
    decimal Amount,
    DateTime DueDate,
    string InvoiceItems,
    string BillingAddress,
    string ClientId);