using ErrorOr;
using Kluster.BusinessModule.Data;
using Kluster.BusinessModule.ServiceErrors;
using Kluster.BusinessModule.Validators;
using Kluster.Shared.Domain;
using Kluster.Shared.DTOs.Requests.Business;
using Kluster.Shared.DTOs.Responses.Business;
using Kluster.Shared.Exceptions;
using Kluster.Shared.Extensions;
using Kluster.Shared.MessagingContracts.Commands.Clients;
using Kluster.Shared.MessagingContracts.Commands.Invoice;
using Kluster.Shared.MessagingContracts.Commands.Payment;
using Kluster.Shared.MessagingContracts.Commands.Products;
using Kluster.Shared.ServiceErrors;
using Kluster.Shared.SharedContracts.BusinessModule;
using Kluster.Shared.SharedContracts.UserModule;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Kluster.BusinessModule.Services;

public class BusinessService(ICurrentUser currentUser, IBus bus, BusinessModuleDbContext context) : IBusinessService
{
    public async Task<ErrorOr<BusinessCreationResponse>> CreateBusinessAsync(CreateBusinessRequest request)
    {
        var validateResult = await new CreateBusinessRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            return validateResult.ToErrorList();
        }

        var userId = currentUser.UserId ?? throw new UserNotSetException();
        if (await context.Businesses.AnyAsync(x => x.UserId == userId))
        {
            return Errors.Business.BusinessAlreadyExists;
        }

        // todo: rename func
        var businessId = await GetBusinessIdFromDb();
        var business = BusinessModuleMapper.ToBusiness(request, userId, businessId);
        await context.AddAsync(business);
        await context.SaveChangesAsync();
        return new BusinessCreationResponse(business.Id);
    }

    private async Task<string> GetBusinessIdFromDb()
    {
        var lastBusiness = await context.Businesses
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();

        if (lastBusiness is null) // only null when no other records exist
        {
            return "B-000001"; // Default ID when no records are present
        }

        var numericId = GetValueFromId(lastBusiness.Id);
        return $"B-{numericId + 1:D6}";
    }

    private static int GetValueFromId(string lastBusinessId)
    {
        var numericPart = lastBusinessId.Substring(2); // Remove the 'B-'
        if (int.TryParse(numericPart, out var numericId))
        {
            return numericId;
        }

        throw new InvalidOperationException("Invalid Business Id");
    }

    public async Task<ErrorOr<GetBusinessResponse>> GetBusinessById(string id)
    {
        var business = await context.Businesses.FindAsync(id);
        if (business is null)
        {
            return SharedErrors<Business>.NotFound;
        }

        return BusinessModuleMapper.ToGetBusinessResponse(business);
    }

    public async Task<ErrorOr<string>> GetBusinessIdOnly()
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();

        var businessId = await context.Businesses.Where(x => x.UserId == userId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

        return businessId is null ? SharedErrors<Business>.NotFound : businessId;
    }

    public async Task<ErrorOr<GetBusinessResponse>> GetBusinessOfLoggedInUser()
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        var business = await context.Businesses.FirstOrDefaultAsync(x => x.UserId == userId);
        if (business is null)
        {
            return SharedErrors<Business>.NotFound;
        }

        return BusinessModuleMapper.ToGetBusinessResponse(business);
    }

    public async Task<ErrorOr<Updated>> UpdateBusiness(UpdateBusinessRequest request)
    {
        var validateResult = await new UpdateBusinessRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            return validateResult.ToErrorList();
        }

        var userId = currentUser.UserId ?? throw new UserNotSetException();
        var business = await context.Businesses.FirstOrDefaultAsync(x => x.UserId == userId);
        if (business is null)
        {
            return SharedErrors<Business>.NotFound;
        }

        business.Name = request.Name ?? business.Name;
        business.Address = request.Address ?? business.Address;
        business.RcNumber = request.RcNumber ?? business.RcNumber;
        business.Description = request.Description ?? business.Description;
        business.Industry = request.Industry ?? business.Industry;

        context.Update(business);
        await context.SaveChangesAsync();
        return Result.Updated;
    }

    public async Task<ErrorOr<Deleted>> DeleteBusiness()
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        var business = await context.Businesses.FirstOrDefaultAsync(x => x.UserId == userId);
        if (business is null)
        {
            return SharedErrors<Business>.NotFound;
        }

        await bus.Publish(new DeletePaymentsForBusiness(business.Id));
        await bus.Publish(new DeleteInvoicesForBusiness(business.Id));
        await bus.Publish(new DeleteClientsForBusiness(business.Id));
        await bus.Publish(new DeleteProductsForBusiness(business.Id));

        context.Remove(business);
        await context.SaveChangesAsync();
        return Result.Deleted;
    }
}