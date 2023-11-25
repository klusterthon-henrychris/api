namespace Kluster.Shared.DTOs.Responses.Product;

public record GetProductResponse(
    string Id,
    string Name,
    string Description,
    decimal Price,
    int Quantity,
    string ImageUrl,
    string ProductType);