using Kluster.Shared.Constants;
using Kluster.Shared.Domain;
using Kluster.Shared.DTOs.Requests.Business;
using Kluster.Shared.DTOs.Requests.Client;
using Kluster.Shared.DTOs.Requests.Product;
using Kluster.Shared.DTOs.Responses.Business;
using Kluster.Shared.DTOs.Responses.Client;
using Kluster.Shared.DTOs.Responses.Product;
using Kluster.Shared.MessagingContracts.Commands.Wallet;

namespace Kluster.BusinessModule;

public static class BusinessModuleMapper
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
            Description = request.BusinessDescription
        };

        return business;
    }

    public static GetBusinessResponse ToGetBusinessResponse(Business business)
    {
        return new GetBusinessResponse(business.Name,
            business.Address,
            business.RcNumber ?? SearchConstants.NotFound,
            business.Description ?? SearchConstants.NotFound,
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
        return new GetClientResponse(client.Id, client.FirstName, client.LastName, client.EmailAddress,
            client.BusinessName ?? string.Join(" ", client.FirstName, client.LastName),
            client.Address);
    }

    public static Product ToProduct(CreateProductRequest request, string businessId, string imageUrl)
    {
        return new Product
        {
            BusinessId = businessId,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            ProductType = request.ProductType,
            Quantity = request.Quantity,
            ImageUrl = imageUrl
        };
    }

    public static GetProductResponse ToGetProductResponse(Product product)
    {
        return new GetProductResponse(product.ProductId, product.Name, product.Description, product.Price,
            product.Quantity,
            product.ImageUrl, product.ProductType);
    }

    public static ClientAndBusinessResponse ToClientAndBusinessResponse(Client client, Business business)
    {
        return new ClientAndBusinessResponse(client.Id, business.Id, client.Address);
    }

    public static Wallet ToWallet(CreateWalletCommand walletCommand)
    {
        return new Wallet
        {
            BusinessId = walletCommand.BusinessId,
            Balance = walletCommand.Balance
        };
    }
}