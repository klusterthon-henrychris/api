namespace Kluster.Shared.DTOs.Responses.Invoices;

public record GetInvoiceResponse(
    string Id,
    string InvoiceNo,
    decimal Amount,
    DateTime DueDate,
    DateTime DateOfIssuance,
    string Status,
    string InvoiceItems);