namespace Kluster.Shared.DTOs.Requests.Product;

// todo: create and update product should be in forms.
public record UpdateProductRequest(
    string? Name,
    string? Description,
    decimal? Price,
    int? Quantity,
    IFormFile? ProductImage,
    string? ProductType);