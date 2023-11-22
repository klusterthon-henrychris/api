using ErrorOr;
using Kluster.BusinessModule.DTOs.Requests;
using Kluster.BusinessModule.DTOs.Requests.Products;
using Kluster.BusinessModule.DTOs.Responses.Products;
using Kluster.Shared.Requests;

namespace Kluster.BusinessModule.Services.Contracts;

public interface IProductService
{
    Task<ErrorOr<CreateProductResponse>> CreateProductAsync(CreateProductRequest request);
    Task<ErrorOr<GetProductResponse>> GetProduct(string id);
    Task<ErrorOr<Updated>> UpdateProduct(string productId, UpdateProductRequest request);
    Task<ErrorOr<PagedList<GetProductResponse>>> GetAllProducts(GetProductsRequest request);
}