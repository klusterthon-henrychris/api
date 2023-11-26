using ErrorOr;
using Kluster.Shared.DTOs.Requests.Product;
using Kluster.Shared.DTOs.Responses.Product;
using Kluster.Shared.DTOs.Responses.Requests;

namespace Kluster.Shared.SharedContracts.BusinessModule;

public interface IProductService
{
    Task<ErrorOr<CreateProductResponse>> CreateProductAsync(CreateProductRequest request);
    Task<ErrorOr<GetProductResponse>> GetProduct(string id);
    Task<ErrorOr<Updated>> UpdateProduct(string productId, UpdateProductRequest request);
    Task<ErrorOr<PagedResponse<GetProductResponse>>> GetAllProducts(GetProductsRequest request);
    Task<ErrorOr<Deleted>> DeleteProduct(string productId);
    Task DeleteAllProductsRelatedToBusiness(string businessId);

    Task<ErrorOr<int>> GetTotalProductsForCurrentUserBusiness();
}