using Kluster.BusinessModule.DTOs.Requests;
using Kluster.BusinessModule.DTOs.Responses;
using Kluster.Shared.API;
using Kluster.Shared.Domain;
using Kluster.Shared.Exceptions;

namespace Kluster.BusinessModule;

public static class Mapper
{
    public static Business ToBusiness(CreateBusinessRequest request, string? userId)
    {
        if (userId is null)
        {
            throw new UserNotSetException("User ID is not set in the current context.");
        }

        var business = new Business
        {
            UserId = userId,
            Name = request.BusinessName,
            Address = request.BusinessAddress,
            Industry = request.Industry,
            RcNumber = request.RcNumber,
            CacNumber = request.CacNumber,
            Description = request.BusinessDescription
        };

        return business;
    }

    public static GetBusinessResponse ToGetBusinessResponse(Business business)
    {
        return new GetBusinessResponse(business.Name,
            business.Address,
            business.CacNumber ?? Constants.NotFound,
            business.RcNumber ?? Constants.NotFound,
            business.Description ?? Constants.NotFound,
            business.Industry);
    }
}