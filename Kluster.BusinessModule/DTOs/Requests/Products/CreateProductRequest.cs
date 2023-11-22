namespace Kluster.BusinessModule.DTOs.Requests.Products;

public record CreateProductRequest(
    string Name,
    string Description,
    decimal Price,
    string ProductType,
    int Quantity,
    IFormFile ProductImage);