using ErrorOr;
using Kluster.PaymentModule.Data;
using Kluster.PaymentModule.Services.Contracts;
using Kluster.PaymentModule.Validators;
using Kluster.Shared.Constants;
using Kluster.Shared.Domain;
using Kluster.Shared.DTOs.Requests.Invoices;
using Kluster.Shared.DTOs.Responses.Invoices;
using Kluster.Shared.DTOs.Responses.Requests;
using Kluster.Shared.Exceptions;
using Kluster.Shared.Extensions;
using Kluster.Shared.ServiceErrors;
using Kluster.Shared.SharedContracts.BusinessModule;
using Kluster.Shared.SharedContracts.UserModule;
using Microsoft.EntityFrameworkCore;

namespace Kluster.PaymentModule.Services;

public class InvoiceService(
    ICurrentUser currentUser,
    IClientService clientService,
    PaymentModuleDbContext context)
    : IInvoiceService
{
    public async Task<ErrorOr<CreateInvoiceResponse>> CreateInvoiceAsync(CreateInvoiceRequest request)
    {
        var validateResult = await new CreateInvoiceRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            return validateResult.ToErrorList();
        }

        // check that the client exists and is register to the business of the authenticated user
        var clientAndBusinessResponse = await clientService.GetClientAndBusiness(request.ClientId);
        if (clientAndBusinessResponse.IsError)
        {
            return clientAndBusinessResponse.FirstError;
        }

        var invoice = PaymentModuleMapper.ToInvoice(request, clientAndBusinessResponse.Value);
        await context.Invoices.AddAsync(invoice);
        await context.SaveChangesAsync();
        return PaymentModuleMapper.ToCreateInvoiceResponse(invoice);
    }

    public async Task<ErrorOr<GetInvoiceResponse>> GetInvoice(string id)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();

        var invoice = await context.Invoices
            .Where(c => c.Business.UserId == userId && c.InvoiceNo == id)
            .FirstOrDefaultAsync();

        return invoice is null ? SharedErrors<Client>.NotFound : PaymentModuleMapper.ToGetInvoiceResponse(invoice);
    }

    public async Task<ErrorOr<Updated>> UpdateInvoice(string invoiceId, UpdateInvoiceRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<PagedList<GetInvoiceResponse>>> GetAllInvoices(GetInvoicesRequest request)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        Enum.TryParse<InvoiceSortOptions>(request.SortOption, out var sortOption);

        var query = context.Invoices.Include(x => x.Business)
            .Where(x => x.Business.UserId == userId);

        query = ApplyFilters(query, request);
        query = SortQuery(query, sortOption);

        var pagedResults = PagedList<GetInvoiceResponse>
            .ToPagedList(
                query.Select(x => new GetInvoiceResponse(
                    x.InvoiceNo,
                    x.Amount,
                    x.DueDate,
                    x.DateOfIssuance,
                    x.Status,
                    x.InvoiceItems
                )),
                request.PageNumber, request.PageSize);

        return Task.FromResult<ErrorOr<PagedList<GetInvoiceResponse>>>(pagedResults);
    }

    private static IQueryable<Invoice> ApplyFilters(IQueryable<Invoice> query, GetInvoicesRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Status))
        {
            return query;
        }

        Enum.TryParse<InvoiceStatus>(request.Status, out var invoiceStatus);
        query = query.Where(x => x.Status.Contains(invoiceStatus.ToString()));

        return query;
    }

    private static IQueryable<Invoice> SortQuery(IQueryable<Invoice> query, InvoiceSortOptions sortOption)
    {
        query = sortOption switch
        {
            InvoiceSortOptions.DueDateAsc => query.OrderBy(x => x.DueDate),
            InvoiceSortOptions.DueDateDesc => query.OrderByDescending(x => x.DueDate),
            InvoiceSortOptions.InvoiceNoAsc => query.OrderBy(x => x.InvoiceNo),
            InvoiceSortOptions.InvoiceNoDesc => query.OrderByDescending(x => x.InvoiceNo),
            InvoiceSortOptions.DateOfIssuanceAsc => query.OrderBy(x => x.DateOfIssuance),
            InvoiceSortOptions.DateOfIssuanceDesc => query.OrderByDescending(x => x.DateOfIssuance),
            InvoiceSortOptions.AmountAsc => query.OrderBy(x => x.Amount),
            InvoiceSortOptions.AmountDesc => query.OrderByDescending(x => x.Amount),

            _ => query.OrderByDescending(x => x.DueDate)
        };

        return query;
    }
}