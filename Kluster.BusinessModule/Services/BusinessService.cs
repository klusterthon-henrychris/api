using ErrorOr;
using Kluster.BusinessModule.Data;
using Kluster.BusinessModule.DTOs.Requests;
using Kluster.BusinessModule.DTOs.Responses;
using Kluster.BusinessModule.ServiceErrors;
using Kluster.BusinessModule.Services.Contracts;
using Kluster.BusinessModule.Validators;
using Kluster.Shared.Extensions;
using Kluster.Shared.SharedContracts.UserModule;

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

        var business = Mapper.ToBusiness(request, currentUser.UserId);
        await context.AddAsync(business);
        await context.SaveChangesAsync();
        return new BusinessCreationResponse(business.Id);
    }

    public async Task<ErrorOr<GetBusinessResponse>> GetBusiness(string id)
    {
        var business = await context.Businesses.FindAsync(id);
        if (business is null)
        {
            return Errors.Business.NotFound;
        }

        return Mapper.ToGetBusinessResponse(business);
    }

    public async Task<ErrorOr<BusinessCreationResponse>> CreateClientBusinessAsync(CreateClientBusinessRequest request)
    {
        var validateResult = await new CreateClientBusinessRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            return validateResult.ToErrorList();
        }

        var business = Mapper.ToBusiness(request);
        await context.AddAsync(business);
        await context.SaveChangesAsync();
        return new BusinessCreationResponse(business.Id);
    }
}