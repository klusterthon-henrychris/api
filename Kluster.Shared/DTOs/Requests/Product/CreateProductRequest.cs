namespace Kluster.Shared.DTOs.Requests.Product;

public record CreateProductRequest(
    string Name,
    string Description,
    decimal Price,
    string ProductType,
    int Quantity,
    IFormFile ProductImage);