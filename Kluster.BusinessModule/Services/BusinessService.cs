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

public class BusinessService(ICurrentUser currentUser, BusinessModuleDbContext context) : IBusinessService
{
    public async Task<ErrorOr<BusinessCreationResponse>> CreateBusinessAsync(CreateBusinessRequest request)
    {
        var validateResult = await new CreateBusinessRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            return validateResult.ToErrorList();
        }

        var userId = currentUser.UserId ?? throw new UserNotSetException("");
        if (await context.Businesses.AnyAsync(x => x.UserId == userId))
        {
            return Errors.Business.BusinessAlreadyExists;
        }

        var business = Mapper.ToBusiness(request, userId);
        await context.AddAsync(business);
        await context.SaveChangesAsync();
        return new BusinessCreationResponse(business.Id);
    }

    public async Task<ErrorOr<GetBusinessResponse>> GetBusinessById(string id)
    {
        var business = await context.Businesses.FindAsync(id);
        if (business is null)
        {
            return SharedErrors<Business>.NotFound;
        }

        return Mapper.ToGetBusinessResponse(business);
    }

    public async Task<ErrorOr<GetBusinessResponse>> GetBusinessOfLoggedInUser()
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException("");
        var business = await context.Businesses.FirstOrDefaultAsync(x => x.UserId == userId);
        if (business is null)
        {
            return SharedErrors<Business>.NotFound;
        }
        
        return Mapper.ToGetBusinessResponse(business);
    }
}