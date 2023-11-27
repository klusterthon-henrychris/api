using ErrorOr;
using Kluster.BusinessModule.Data;
using Kluster.BusinessModule.ServiceErrors;
using Kluster.BusinessModule.Validators;
using Kluster.Shared.Constants;
using Kluster.Shared.Domain;
using Kluster.Shared.DTOs.Requests.Client;
using Kluster.Shared.DTOs.Responses.Client;
using Kluster.Shared.DTOs.Responses.Requests;
using Kluster.Shared.Exceptions;
using Kluster.Shared.Extensions;
using Kluster.Shared.MessagingContracts.Commands.Invoice;
using Kluster.Shared.MessagingContracts.Commands.Payment;
using Kluster.Shared.ServiceErrors;
using Kluster.Shared.SharedContracts.BusinessModule;
using Kluster.Shared.SharedContracts.UserModule;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Kluster.BusinessModule.Services;

public class ClientService(
    ICurrentUser currentUser,
    ILogger<ClientService> logger,
    IBus bus,
    BusinessModuleDbContext context) : IClientService
{
    public async Task<ErrorOr<GetClientResponse>> GetClient(string id)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        logger.LogInformation("Fetching client with id: {id}. Request from user: {userId}", id, userId);

        var client = await context.Clients
            .Where(c => c.Business.UserId == userId && c.Id == id)
            .FirstOrDefaultAsync();

        if (client is null)
        {
            logger.LogError("Client not found, or does not belong to business owned by {userId}.", userId);
            return SharedErrors<Client>.NotFound;
        }

        logger.LogInformation("Successfully retrieved client.");
        return BusinessModuleMapper.ToGetClientResponse(client);
    }

    public async Task<ErrorOr<CreateClientResponse>> CreateClientAsync(CreateClientRequest request)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        logger.LogInformation("Received request to create client from {userId}.\nRequest: {request}", userId, request);
        var validateResult = await new CreateClientRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            logger.LogError("CreateClient request failed - A validation error occurred.");
            return validateResult.ToErrorList();
        }

        var businessId = await context.Businesses
            .Where(x => x.UserId == userId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

        if (businessId is null)
        {
            logger.LogError("CreateClient request failed - business {businessId} does not exist.", businessId);
            return SharedErrors<Business>.NotFound;
        }

        var client = BusinessModuleMapper.ToClient(request, businessId);
        await context.Clients.AddAsync(client);
        await context.SaveChangesAsync();
        logger.LogInformation("Successfully created client {clientId} under business {businessId}.", client.Id, businessId);
        return new CreateClientResponse(client.Id);
    }

    public async Task<ErrorOr<PagedResponse<GetClientResponse>>> GetAllClients(GetClientsRequest request)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        logger.LogInformation("GetAllClients method started for user {userId}.", userId);
        Enum.TryParse<ClientSortOptions>(request.SortOption, out var sortOption);

        var query = context.Clients
            .Include(x => x.Business)
            .Where(x => x.Business.UserId == userId);

        query = ApplyFilters(query, request);
        query = SortClientsQuery(query, sortOption);

        var pagedResults =
            query.Select(x => new GetClientResponse(
                x.Id,
                x.FirstName,
                x.LastName,
                x.EmailAddress,
                x.BusinessName ?? string.Join(" ", x.FirstName, x.LastName),
                x.Address));

        var response = await new PagedResponse<GetClientResponse>().ToPagedList(pagedResults, request.PageNumber,
            request.PageSize);
        logger.LogInformation("GetAllClients method completed for user {userId}. Returned {totalCount} clients.", userId, response.TotalCount);
        return response;
    }

    private IQueryable<Client> ApplyFilters(IQueryable<Client> query, GetClientsRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.FirstName))
        {
            logger.LogInformation("Applied filter for FirstName: {firstName}", request.FirstName);
            query = query.Where(x => x.FirstName.Contains(request.FirstName));
        }

        if (!string.IsNullOrWhiteSpace(request.LastName))
        {
            logger.LogInformation("Applied filter for LastName: {lastName}", request.LastName);
            query = query.Where(x => x.LastName.Contains(request.LastName));
        }

        if (!string.IsNullOrWhiteSpace(request.BusinessName))
        {
            logger.LogInformation("Applied filter for BusinessName: {businessName}", request.BusinessName);
            query = query.Where(x => x.BusinessName != null && x.BusinessName.Contains(request.BusinessName));
        }

        if (!string.IsNullOrWhiteSpace(request.EmailAddress))
        {
            logger.LogInformation("Applied filter for EmailAddress: {emailAddress}", request.EmailAddress);
            query = query.Where(x => x.EmailAddress.Contains(request.EmailAddress));
        }

        if (!string.IsNullOrWhiteSpace(request.Address))
        {
            logger.LogInformation("Applied filter for Address: {address}", request.Address);
            query = query.Where(x => x.Address.Contains(request.Address));
        }

        return query;
    }

    private IQueryable<Client> SortClientsQuery(IQueryable<Client> query, ClientSortOptions sortOption)
    {
        logger.LogInformation("Sorting clients by: {sortOption}", sortOption);
        query = sortOption switch
        {
            ClientSortOptions.FirstNameAsc => query.OrderBy(x => x.FirstName),
            ClientSortOptions.FirstNameDesc => query.OrderByDescending(x => x.FirstName),
            ClientSortOptions.LastNameAsc => query.OrderBy(x => x.LastName),
            ClientSortOptions.LastNameDesc => query.OrderByDescending(x => x.LastName),
            ClientSortOptions.BusinessNameAsc => query.OrderBy(x => x.BusinessName),
            ClientSortOptions.BusinessNameDesc => query.OrderByDescending(x => x.BusinessName),
            ClientSortOptions.CreatedDateAsc => query.OrderBy(x => x.CreatedDate),
            ClientSortOptions.CreatedDateDesc => query.OrderByDescending(x => x.CreatedDate),

            _ => query.OrderBy(x => x.FirstName)
        };

        return query;
    }

    public async Task<ErrorOr<Updated>> UpdateClient(string clientId, UpdateClientRequest request)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        logger.LogInformation("Received request to update client {clientId} from {userId}.\nRequest: {request}", clientId, userId, request);
        var validateResult = await new UpdateClientRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            logger.LogError("UpdateClient request failed - A validation error occurred.");
            return validateResult.ToErrorList();
        }

        var client = await context.Clients
            .Include(client => client.Business)
            .FirstOrDefaultAsync(x => x.Id == clientId);

        if (client is null)
        {
            logger.LogError("Client {clientId} not found.", clientId);
            return SharedErrors<Client>.NotFound;
        }

        if (client.Business.UserId != userId)
        {
            logger.LogError("Client does not belong to business owned by {userId}.", userId);
            return Errors.Client.InvalidBusiness;
        }

        client.FirstName = request.FirstName ?? client.FirstName;
        client.BusinessName = request.BusinessName ?? client.BusinessName;
        client.LastName = request.LastName ?? client.LastName;
        client.Address = request.Address ?? client.Address;
        client.EmailAddress = request.EmailAddress ?? client.EmailAddress;

        context.Update(client);
        await context.SaveChangesAsync();
        logger.LogInformation("Client {clientId} has been updated.", clientId);
        return Result.Updated;
    }

    public async Task<ErrorOr<ClientAndBusinessResponse>> GetClientAndBusiness(string clientId)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();

        var client = await context.Clients
            .Include(client => client.Business)
            .FirstOrDefaultAsync(x => x.Id == clientId);

        if (client is null)
        {
            return SharedErrors<Client>.NotFound;
        }

        if (client.Business.UserId != userId)
        {
            return Errors.Client.InvalidBusiness;
        }

        return BusinessModuleMapper.ToClientAndBusinessResponse(client, client.Business);
    }

    public async Task DeleteAllClientsRelatedToBusiness(string businessId)
    {
        var clients = await context.Clients
            .Where(x => x.BusinessId == businessId)
            .ToListAsync();

        logger.LogInformation("Deleting {clientCount} client(s) for business {businessId}.", clients.Count, businessId);
        context.RemoveRange(clients);
        await context.SaveChangesAsync();
        logger.LogInformation("Clients have been successfully deleted.");
    }

    public async Task<ErrorOr<Deleted>> DeleteClient(string clientId)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        logger.LogInformation("Received request to delete client from {userId}.", userId);
        var client = await context.Clients.FirstOrDefaultAsync(x => x.Id == clientId && x.Business.UserId == userId);
        if (client is null)
        {
            logger.LogError("Client {clientId} does not exist, or does not belong to business owned by {userId}.", clientId, userId);
            return SharedErrors<Client>.NotFound;
        }

        logger.LogInformation("Queueing requests to delete related payments and invoices for {clientId}.", clientId);
        await bus.Publish(new DeletePaymentsForClient(clientId));
        await bus.Publish(new DeleteInvoicesForClient(clientId));

        context.Remove(client);
        await context.SaveChangesAsync();
        logger.LogInformation("Client {clientId} has been deleted.", clientId);
        return Result.Deleted;
    }

    public async Task<ErrorOr<int>> GetTotalClientsForCurrentUserBusiness()
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        return await context.Clients.Where(x => x.Business.UserId == userId).CountAsync();
    }
}