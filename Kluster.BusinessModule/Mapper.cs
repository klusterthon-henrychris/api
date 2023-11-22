using ErrorOr;
using Kluster.BusinessModule.DTOs.Requests;
using Kluster.BusinessModule.DTOs.Responses;
using Kluster.Shared.API;
using Kluster.Shared.Domain;

namespace Kluster.BusinessModule;

public static class Mapper
{
    public static Business ToBusiness(CreateBusinessRequest request, string userId, string businessId)
    {
        var business = new Business
        {
            Id = businessId,
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

    public static Client ToClient(CreateClientRequest request, string businessId)
    {
        var businessName = request.BusinessName;
        if (string.IsNullOrEmpty(request.BusinessName))
        {
            businessName = string.Join(" ", request.FirstName, request.LastName);
        }

        var client = new Client
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Address = request.Address,
            EmailAddress = request.EmailAddress,
            BusinessName = businessName,
            BusinessId = businessId
        };

        return client;
    }

    public static GetClientResponse ToGetClientResponse(Client client)
    {
        return new GetClientResponse(client.FirstName, client.LastName, client.EmailAddress,
            client.BusinessName ?? string.Join(" ", client.FirstName, client.LastName),
            client.Address);
    }
}