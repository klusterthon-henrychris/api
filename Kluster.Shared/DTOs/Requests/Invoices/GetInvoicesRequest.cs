using Kluster.Shared.Constants;
using Kluster.Shared.DTOs.Responses.Requests;

namespace Kluster.Shared.DTOs.Requests.Invoices;

public class GetInvoicesRequest : QueryStringParameters
{
    public string? Status { get; set; }
    public string SortOption { get; set; } = InvoiceSortStrings.DueDateDesc;
}