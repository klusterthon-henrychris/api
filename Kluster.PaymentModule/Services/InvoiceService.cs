using ErrorOr;
using Kluster.PaymentModule.Data;
using Kluster.PaymentModule.Validators;
using Kluster.Shared.Constants;
using Kluster.Shared.Domain;
using Kluster.Shared.DTOs.Requests.Invoices;
using Kluster.Shared.DTOs.Responses.Invoices;
using Kluster.Shared.DTOs.Responses.Requests;
using Kluster.Shared.Extensions;
using Kluster.Shared.MessagingContracts.Commands.Invoice;
using Kluster.Shared.MessagingContracts.Events.Invoices;
using Kluster.Shared.ServiceErrors;
using Kluster.Shared.SharedContracts.BusinessModule;
using Kluster.Shared.SharedContracts.PaymentModule;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Kluster.PaymentModule.Services;

public class InvoiceService(
    IClientService clientService,
    IBusinessService businessService,
    IBus bus,
    ILogger<InvoiceService> logger,
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
        
        // create incomplete payment
        await bus.Publish(new InvoiceCreatedEvent(invoice.BusinessId, invoice.ClientId, invoice.InvoiceNo, invoice.Amount));
        
        return PaymentModuleMapper.ToCreateInvoiceResponse(invoice);
    }

    public async Task<ErrorOr<GetInvoiceResponse>> GetInvoice(string invoiceNo)
    {
        var businessIdOfCurrentUser = await businessService.GetBusinessIdOnlyForCurrentUser();
        if (businessIdOfCurrentUser.IsError)
        {
            return businessIdOfCurrentUser.Errors;
        }

        var invoice = await context.Invoices
            .Where(i => i.BusinessId == businessIdOfCurrentUser.Value && i.InvoiceNo == invoiceNo)
            .FirstOrDefaultAsync();

        return invoice is null ? SharedErrors<Invoice>.NotFound : PaymentModuleMapper.ToGetInvoiceResponse(invoice);
    }

    public Task<ErrorOr<Updated>> UpdateInvoice(string invoiceId, UpdateInvoiceRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<ErrorOr<PagedResponse<GetInvoiceResponse>>> GetAllInvoices(GetInvoicesRequest request)
    {
        Enum.TryParse<InvoiceSortOptions>(request.SortOption, out var sortOption);

        var businessIdOfCurrentUser = await businessService.GetBusinessIdOnlyForCurrentUser();
        if (businessIdOfCurrentUser.IsError)
        {
            return businessIdOfCurrentUser.Errors;
        }

        var query = context.Invoices.Where(x => x.BusinessId == businessIdOfCurrentUser.Value);
        query = ApplyStatusFilters(query, request.Status);
        query = SortQuery(query, sortOption);

        var pagedResults =
            query.Select(x => new GetInvoiceResponse(
                x.InvoiceNo,
                x.Amount,
                x.DueDate,
                x.DateOfIssuance,
                x.Status,
                x.InvoiceItems
            ));

        return await new PagedResponse<GetInvoiceResponse>().ToPagedList(pagedResults, request.PageNumber,
            request.PageSize);
    }

    #region Delete Invoices

    public async Task<ErrorOr<Deleted>> DeleteSingleInvoice(string id)
    {
        var businessIdOfCurrentUser = await businessService.GetBusinessIdOnlyForCurrentUser();
        if (businessIdOfCurrentUser.IsError)
        {
            return businessIdOfCurrentUser.Errors;
        }

        var invoice = await context.Invoices
            .Where(c => c.BusinessId == businessIdOfCurrentUser.Value && c.InvoiceNo == id)
            .FirstOrDefaultAsync();

        if (invoice is null)
        {
            return SharedErrors<Invoice>.NotFound;
        }

        // delete related payments
        await bus.Publish(PaymentModuleMapper.ToDeletePaymentForInvoice(invoice));

        context.Remove(invoice);
        await context.SaveChangesAsync();
        return Result.Deleted;
    }

    public async Task DeleteAllInvoicesLinkedToClient(DeleteInvoicesForClient command)
    {
        var invoices = await context.Invoices
            .Where(x => x.ClientId == command.ClientId)
            .ToListAsync();

        context.RemoveRange(invoices);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAllInvoicesLinkedToBusiness(DeleteInvoicesForBusiness command)
    {
        var invoices = await context.Invoices
            .Where(x => x.ClientId == command.BusinessId)
            .ToListAsync();

        context.RemoveRange(invoices);
        await context.SaveChangesAsync();
    }

    public async Task<ErrorOr<int>> GetInvoiceCountForCurrentUserBusiness(string? filter)
    {
        var businessIdOfCurrentUser = await businessService.GetBusinessIdOnlyForCurrentUser();
        if (businessIdOfCurrentUser.IsError)
        {
            logger.LogError(
                $"Request: {nameof(GetInvoiceCountForCurrentUserBusiness)}. Unable to fetch businessId, returning 0.");
            return 0;
        }

        var invoicesQuery = context.Invoices
            .Where(c => c.BusinessId == businessIdOfCurrentUser.Value);

        if (filter is not null)
        {
            invoicesQuery = ApplyStatusFilters(invoicesQuery, filter);
        }

        return await invoicesQuery.CountAsync();
    }

    #endregion

    #region Filter and Query

    private static IQueryable<Invoice> ApplyStatusFilters(IQueryable<Invoice> query, string? invoiceStatus)
    {
        if (string.IsNullOrWhiteSpace(invoiceStatus))
        {
            return query;
        }

        Enum.TryParse<InvoiceStatus>(invoiceStatus, out var invoiceStatusEnum);
        query = query.Where(x =>
            x.Status.Equals(invoiceStatusEnum.ToString(), StringComparison.CurrentCultureIgnoreCase));

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

    #endregion
}