using ErrorOr;
using Kluster.Shared.Domain;
using Kluster.Shared.DTOs.Requests.Invoices;
using Kluster.Shared.DTOs.Responses.Invoices;
using Kluster.Shared.DTOs.Responses.Requests;
using Kluster.Shared.MessagingContracts.Commands.Invoice;

namespace Kluster.Shared.SharedContracts.PaymentModule;

public interface IInvoiceService
{
    Task<ErrorOr<CreateInvoiceResponse>> CreateInvoiceAsync(CreateInvoiceRequest request);
    Task<ErrorOr<GetInvoiceResponse>> GetInvoice(string invoiceNo);
    Task<ErrorOr<Updated>> UpdateInvoice(string invoiceId, UpdateInvoiceRequest request);
    Task<ErrorOr<PagedResponse<GetInvoiceResponse>>> GetAllInvoices(GetInvoicesRequest request);
    Task<ErrorOr<Deleted>> DeleteSingleInvoice(string id);
    Task DeleteAllInvoicesLinkedToClient(DeleteInvoicesForClient command);
    Task DeleteAllInvoicesLinkedToBusiness(DeleteInvoicesForBusiness command);

    Task<ErrorOr<int>> GetInvoiceCountForCurrentUserBusiness(string? filter);
    
    /// <summary>
    /// Used by reminder service. If the invoice doesn't exist, it couldn't have gotten there.
    /// </summary>
    /// <param name="invoiceNo"></param>
    /// <returns></returns>
    Task<Invoice> GetInvoiceInternal(string invoiceNo);
}