namespace Kluster.BusinessModule.DTOs.Responses.Products;

public record GetProductResponse(
    string Name,
    string Description,
    decimal Price,
    int Quantity,
    string ImageUrl,
    string ProductType);