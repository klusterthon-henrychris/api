namespace Kluster.Shared.DTOs.Responses.Invoices;

public record GetInvoiceResponse(
    string Id,
    decimal Amount,
    DateTime DueDate,
    DateTime DateOfIssuance,
    string Status,
    string InvoiceItems, string ClientId);