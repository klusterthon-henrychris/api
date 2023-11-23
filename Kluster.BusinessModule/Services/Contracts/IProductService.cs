using ErrorOr;
using Kluster.Shared.DTOs.Requests.Product;
using Kluster.Shared.DTOs.Responses.Product;
using Kluster.Shared.DTOs.Responses.Requests;

namespace Kluster.BusinessModule.Services.Contracts;

public interface IProductService
{
    Task<ErrorOr<CreateProductResponse>> CreateProductAsync(CreateProductRequest request);
    Task<ErrorOr<GetProductResponse>> GetProduct(string id);
    Task<ErrorOr<Updated>> UpdateProduct(string productId, UpdateProductRequest request);
    Task<ErrorOr<PagedList<GetProductResponse>>> GetAllProducts(GetProductsRequest request);
}