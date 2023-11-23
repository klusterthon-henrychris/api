namespace Kluster.Shared.DTOs.Responses.Product;

public record GetProductResponse(
    string Name,
    string Description,
    decimal Price,
    int Quantity,
    string ImageUrl,
    string ProductType);