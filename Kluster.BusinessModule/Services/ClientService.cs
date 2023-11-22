using ErrorOr;
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

    public async Task<ErrorOr<List<GetClientResponse>>> GetAllClients()
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();

        // todo: this is probably slow. maybe get only what is needed. even, if it means another db trip.
        var business = await context.Businesses
            .Where(x => x.UserId == userId)
            .Include(x => x.Clients).FirstOrDefaultAsync();
        if (business is null)
        {
            return SharedErrors<Business>.NotFound;
        }

        return business.Clients.Select(Mapper.ToGetClientResponse).ToList();
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