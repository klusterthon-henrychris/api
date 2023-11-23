using ErrorOr;
using Kluster.Shared.DTOs.Requests.Invoices;
using Kluster.Shared.DTOs.Responses.Invoices;
using Kluster.Shared.DTOs.Responses.Requests;

namespace Kluster.PaymentModule.Services.Contracts;

public interface IInvoiceService
{
    Task<ErrorOr<CreateInvoiceResponse>> CreateInvoiceAsync(CreateInvoiceRequest request);
    Task<ErrorOr<GetInvoiceResponse>> GetInvoice(string id);
    Task<ErrorOr<Updated>> UpdateInvoice(string invoiceId, UpdateInvoiceRequest request);
    Task<ErrorOr<PagedList<GetInvoiceResponse>>> GetAllInvoices(GetInvoicesRequest request);
}