using ErrorOr;
using Kluster.BusinessModule.Constants;
using Kluster.BusinessModule.Data;
using Kluster.BusinessModule.DTOs.Requests;
using Kluster.BusinessModule.DTOs.Responses;
using Kluster.BusinessModule.ServiceErrors;
using Kluster.BusinessModule.Services.Contracts;
using Kluster.BusinessModule.Validators;
using Kluster.Shared.Domain;
using Kluster.Shared.Exceptions;
using Kluster.Shared.Extensions;
using Kluster.Shared.ServiceErrors;
using Kluster.Shared.SharedContracts.UserModule;
using Microsoft.EntityFrameworkCore;

namespace Kluster.BusinessModule.Services;

public class ClientService(ICurrentUser currentUser, BusinessModuleDbContext context) : IClientService
{
    public async Task<ErrorOr<GetClientResponse>> GetClient(string id)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();

        var client = await context.Clients
            .Where(c => c.Business.UserId == userId && c.Id == id)
            .FirstOrDefaultAsync();

        return client is null ? SharedErrors<Client>.NotFound : Mapper.ToGetClientResponse(client);
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

        var client = Mapper.ToClient(request, businessId);
        await context.Clients.AddAsync(client);
        await context.SaveChangesAsync();
        return new CreateClientResponse(client.Id);
    }

    public async Task<ErrorOr<List<GetClientResponse>>> GetAllClients(GetClientsRequest request)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        Enum.TryParse<ClientSortOptions>(request.SortOption, out var sortOption);

        var query = context.Clients
            .Include(x => x.Business)
            .Where(x => x.Business.UserId == userId);

        query = SortClientsQuery(query, sortOption);
        var clients = await query
            .Skip(request.PageSize * (request.PageNumber - 1))
            .Take(request.PageSize)
            .ToListAsync();
        
        return clients.Select(Mapper.ToGetClientResponse).ToList();
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

            _ => query.OrderBy(x => x.CreatedDate)
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
}