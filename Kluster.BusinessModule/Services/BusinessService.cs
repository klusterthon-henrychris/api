using ErrorOr;
using Kluster.BusinessModule.Data;
using Kluster.BusinessModule.ServiceErrors;
using Kluster.BusinessModule.Validators;
using Kluster.Shared.Domain;
using Kluster.Shared.DTOs.Requests.Business;
using Kluster.Shared.DTOs.Requests.Wallet;
using Kluster.Shared.DTOs.Responses.Business;
using Kluster.Shared.Exceptions;
using Kluster.Shared.Extensions;
using Kluster.Shared.MessagingContracts.Commands.Clients;
using Kluster.Shared.MessagingContracts.Commands.Invoice;
using Kluster.Shared.MessagingContracts.Commands.Payment;
using Kluster.Shared.MessagingContracts.Commands.Products;
using Kluster.Shared.MessagingContracts.Commands.Wallet;
using Kluster.Shared.ServiceErrors;
using Kluster.Shared.SharedContracts.BusinessModule;
using Kluster.Shared.SharedContracts.UserModule;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Kluster.BusinessModule.Services;

public class BusinessService(
    ICurrentUser currentUser,
    ILogger<BusinessService> logger,
    IBus bus,
    BusinessModuleDbContext context) : IBusinessService
{
    public async Task<ErrorOr<BusinessCreationResponse>> CreateBusinessForCurrentUser(CreateBusinessRequest request)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();

        logger.LogInformation("Received request to create business. Request: {0}", request);
        var validateResult = await new CreateBusinessRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            logger.LogError("CreateBusinessRequest failed - A validation error occured.");
            return validateResult.ToErrorList();
        }

        if (await context.Businesses.AnyAsync(x => x.UserId == userId))
        {
            logger.LogError("CreateBusinessRequest failed - {0}", Errors.Business.BusinessAlreadyExists.Description);
            return Errors.Business.BusinessAlreadyExists;
        }

        var businessId = await GetBusinessIdFromDb();
        var business = BusinessModuleMapper.ToBusiness(request, userId, businessId);
        await context.AddAsync(business);
        await context.SaveChangesAsync();

        logger.LogInformation("Successfully created business {0} for {1}.", business.Id, userId);
        await bus.Publish(new CreateWalletCommand(businessId, 0));
        return new BusinessCreationResponse(business.Id);
    }

    /// <summary>
    /// Gets a new business ID, based on the last one in the database.
    /// </summary>
    /// <returns></returns>
    private async Task<string> GetBusinessIdFromDb()
    {
        var lastBusiness = await context.Businesses
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();

        if (lastBusiness is null) // only null when no other records exist
        {
            logger.LogInformation("No other business entities exist. Returning 001.");
            return "B-000001"; // Default ID when no records are present
        }

        var numericId = GetValueFromId(lastBusiness.Id);
        logger.LogInformation("Previous businessId: {0}. New Id: B-{1:D6}", lastBusiness.Id, numericId + 1);
        return $"B-{numericId + 1:D6}";
    }

    private static int GetValueFromId(string lastBusinessId)
    {
        var numericPart = lastBusinessId.Substring(2); // Remove the 'B-'
        if (int.TryParse(numericPart, out var numericId))
        {
            return numericId;
        }

        throw new InvalidOperationException("Invalid Business Id.");
    }

    public async Task<ErrorOr<GetBusinessResponse>> GetBusinessById(string id)
    {
        var business = await context.Businesses.FindAsync(id);
        if (business is null)
        {
            logger.LogError("Business {0} does not exist.", id);
            return SharedErrors<Business>.NotFound;
        }

        logger.LogInformation("Retrieved business {0}.", business.Id);
        return BusinessModuleMapper.ToGetBusinessResponse(business);
    }

    public async Task<ErrorOr<string>> GetBusinessIdOnlyForCurrentUser()
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();

        var businessId = await context.Businesses.Where(x => x.UserId == userId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

        return businessId is null ? SharedErrors<Business>.NotFound : businessId;
    }

    public async Task<ErrorOr<GetBusinessResponse>> GetBusinessForCurrentUser()
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        var business = await context.Businesses.FirstOrDefaultAsync(x => x.UserId == userId);
        if (business is null)
        {
            logger.LogError("User {0} does not have a business.", userId);
            return SharedErrors<Business>.NotFound;
        }

        logger.LogInformation("Retrieved business for {0}", userId);
        return BusinessModuleMapper.ToGetBusinessResponse(business);
    }

    public async Task<ErrorOr<Updated>> UpdateBusinessForCurrentUser(UpdateBusinessRequest request)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        logger.LogInformation("Received request to update business from user: {0}.\nRequest: {1}", userId, request);

        var validateResult = await new UpdateBusinessRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            logger.LogError("UpdateBusinessRequest failed: A validation error occurred.");
            return validateResult.ToErrorList();
        }

        var business = await context.Businesses.FirstOrDefaultAsync(x => x.UserId == userId);
        if (business is null)
        {
            logger.LogError("User {0} does not have a business.", userId);
            return SharedErrors<Business>.NotFound;
        }

        business.Name = request.Name ?? business.Name;
        business.Address = request.Address ?? business.Address;
        business.RcNumber = request.RcNumber ?? business.RcNumber;
        business.Description = request.Description ?? business.Description;
        business.Industry = request.Industry ?? business.Industry;

        context.Update(business);
        await context.SaveChangesAsync();
        logger.LogInformation("Business {0} has been updated.", business.Id);
        return Result.Updated;
    }

    public async Task<ErrorOr<Deleted>> DeleteBusinessForCurrentUser()
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        logger.LogInformation("Received DeleteBusiness request from {0}.", userId);
        var business = await context.Businesses.FirstOrDefaultAsync(x => x.UserId == userId);
        if (business is null)
        {
            logger.LogError("User {0} does not have a business.", userId);
            return SharedErrors<Business>.NotFound;
        }

        logger.LogInformation("Queued requests to delete related entities.");
        await bus.Publish(new DeletePaymentsForBusiness(business.Id));
        await bus.Publish(new DeleteInvoicesForBusiness(business.Id));
        await bus.Publish(new DeleteClientsForBusiness(business.Id));
        await bus.Publish(new DeleteProductsForBusiness(business.Id));

        context.Remove(business);
        await context.SaveChangesAsync();
        logger.LogInformation("Business {0} deleted", business.Id);
        return Result.Deleted;
    }

    public async Task<ErrorOr<GetWalletBalanceResponse>> GetBusinessWalletBalance()
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        logger.LogInformation("Fetching wallet balance for {0}.", userId);
        var business = await context.Businesses
            .Include(business => business.Wallet)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (business is null)
        {
            logger.LogError("User {0} does not have a business.", userId);
            return SharedErrors<Business>.NotFound;
        }

        if (business.Wallet is null)
        {
            logger.LogError("A wallet has not been created for {0}.", business.Id);
            return Errors.Business.WalletNotCreated;
        }

        logger.LogInformation("Wallet balance retrieved for {0}", business.Id);
        return new GetWalletBalanceResponse(business.Id, business.Name, business.Wallet.Balance);
    }

    public async Task<string?> GetBusinessName(string businessId)
    {
        return await context.Businesses.Where(x => x.Id == businessId)
            .Select(x => x.Name)
            .FirstOrDefaultAsync();
    }
}