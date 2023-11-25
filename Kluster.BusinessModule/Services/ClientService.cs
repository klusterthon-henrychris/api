﻿using ErrorOr;
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

public class ClientService(ICurrentUser currentUser, IBus bus, BusinessModuleDbContext context) : IClientService
{
    public async Task<ErrorOr<GetClientResponse>> GetClient(string id)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();

        var client = await context.Clients
            .Where(c => c.Business.UserId == userId && c.Id == id)
            .FirstOrDefaultAsync();

        return client is null ? SharedErrors<Client>.NotFound : BusinessModuleMapper.ToGetClientResponse(client);
    }

    public async Task<ErrorOr<CreateClientResponse>> CreateClientAsync(CreateClientRequest request)
    {
        var validateResult = await new CreateClientRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            return validateResult.ToErrorList();
        }

        var userId = currentUser.UserId ?? throw new UserNotSetException();
        var businessId = await context.Businesses
            .Where(x => x.UserId == userId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

        if (businessId is null)
        {
            return SharedErrors<Business>.NotFound;
        }

        var client = BusinessModuleMapper.ToClient(request, businessId);
        await context.Clients.AddAsync(client);
        await context.SaveChangesAsync();
        return new CreateClientResponse(client.Id);
    }

    public Task<ErrorOr<PagedList<GetClientResponse>>> GetAllClients(GetClientsRequest request)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        Enum.TryParse<ClientSortOptions>(request.SortOption, out var sortOption);

        var query = context.Clients
            .Include(x => x.Business)
            .Where(x => x.Business.UserId == userId);

        query = ApplyFilters(query, request);
        query = SortClientsQuery(query, sortOption);
        var pagedResults = PagedList<GetClientResponse>
            .ToPagedList(
                query.Select(x =>
                    new GetClientResponse(
                        x.Id,
                        x.FirstName,
                        x.LastName,
                        x.EmailAddress,
                        x.BusinessName ?? string.Join(" ", x.FirstName, x.LastName),
                        x.Address)),
                request.PageNumber,
                request.PageSize
            );

        return Task.FromResult<ErrorOr<PagedList<GetClientResponse>>>(pagedResults);
    }

    private static IQueryable<Client> ApplyFilters(IQueryable<Client> query, GetClientsRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.FirstName))
        {
            query = query.Where(x => x.FirstName.Contains(request.FirstName));
        }

        if (!string.IsNullOrWhiteSpace(request.LastName))
        {
            query = query.Where(x => x.LastName.Contains(request.LastName));
        }

        if (!string.IsNullOrWhiteSpace(request.BusinessName))
        {
            query = query.Where(x => x.BusinessName != null && x.BusinessName.Contains(request.BusinessName));
        }

        if (!string.IsNullOrWhiteSpace(request.EmailAddress))
        {
            query = query.Where(x => x.EmailAddress.Contains(request.EmailAddress));
        }

        if (!string.IsNullOrWhiteSpace(request.Address))
        {
            query = query.Where(x => x.Address.Contains(request.Address));
        }

        return query;
    }

    private static IQueryable<Client> SortClientsQuery(IQueryable<Client> query, ClientSortOptions sortOption)
    {
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
        var validateResult = await new UpdateClientRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            return validateResult.ToErrorList();
        }

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

        client.FirstName = request.FirstName ?? client.FirstName;
        client.BusinessName = request.BusinessName ?? client.BusinessName;
        client.LastName = request.LastName ?? client.LastName;
        client.Address = request.Address ?? client.Address;
        client.EmailAddress = request.EmailAddress ?? client.EmailAddress;

        context.Update(client);
        await context.SaveChangesAsync();
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

        context.RemoveRange(clients);
        await context.SaveChangesAsync();
    }

    public async Task<ErrorOr<Deleted>> DeleteClient(string clientId)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        var client = await context.Clients.FirstOrDefaultAsync(x => x.Id == clientId && x.Business.UserId == userId);
        if (client is null)
        {
            return SharedErrors<Client>.NotFound;
        }

        await bus.Publish(new DeletePaymentsForClient(clientId));
        await bus.Publish(new DeleteInvoicesForClient(clientId));
        
        context.Remove(client);
        await context.SaveChangesAsync();
        return Result.Deleted;
    }

    public async Task<ErrorOr<int>> GetTotalClientsForCurrentUserBusiness()
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        return await context.Clients.Where(x => x.Business.UserId == userId).CountAsync();
    }
}