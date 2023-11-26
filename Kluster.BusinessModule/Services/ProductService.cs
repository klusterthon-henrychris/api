using ErrorOr;
using Kluster.BusinessModule.Data;
using Kluster.BusinessModule.ServiceErrors;
using Kluster.BusinessModule.Validators;
using Kluster.BusinessModule.Validators.Helpers;
using Kluster.Shared.Constants;
using Kluster.Shared.Domain;
using Kluster.Shared.DTOs.Requests.Product;
using Kluster.Shared.DTOs.Responses.Product;
using Kluster.Shared.DTOs.Responses.Requests;
using Kluster.Shared.Exceptions;
using Kluster.Shared.Extensions;
using Kluster.Shared.ServiceErrors;
using Kluster.Shared.SharedContracts.BusinessModule;
using Kluster.Shared.SharedContracts.UserModule;
using Microsoft.EntityFrameworkCore;

namespace Kluster.BusinessModule.Services;

public class ProductService(ICurrentUser currentUser, BusinessModuleDbContext context, ILogger<ProductService> logger)
    : IProductService
{
    public async Task<ErrorOr<CreateProductResponse>> CreateProductAsync(CreateProductRequest request)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        logger.LogInformation($"Received request to create product from {userId}.\n" +
                              $"Request: {request}");
        var validateResult = await new CreateProductRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            logger.LogError("Validation failed for CreateProductRequest");
            return validateResult.ToErrorList();
        }

        var businessId = await context.Businesses
            .Where(x => x.UserId == userId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

        if (businessId is null)
        {
            logger.LogError($"Business not found for user: {userId}");
            return SharedErrors<Business>.NotFound;
        }

        var imageUrl = await UploadImageToS3(request.ProductImage);

        var product = BusinessModuleMapper.ToProduct(request, businessId, imageUrl!);
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        logger.LogInformation($"Product created successfully. ProductId: {product.ProductId}.");

        return new CreateProductResponse(product.ProductId);
    }

    private Task<string?> UploadImageToS3(IFormFile requestProductImage)
    {
        // todo: find a place to upload images.
        // todo: remove nullable suppressor later
        return Task.FromResult(
            "https://fastly.picsum.photos/id/826/200/200.jpg?hmac=WlCuCjxEhXh_s4IkOpulPoB-LOoGjfZwP4GjNnkzTLA")!;
    }

    public async Task<ErrorOr<GetProductResponse>> GetProduct(string id)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        logger.LogInformation($"Received request to get product {id} for user {userId}.");

        var product = await context.Products
            .Where(c => c.Business.UserId == userId && c.ProductId == id)
            .FirstOrDefaultAsync();

        if (product is null)
        {
            logger.LogError($"Product {id} not found for user {userId}.");
            return SharedErrors<Product>.NotFound;
        }

        logger.LogInformation($"Product {id} retrieved successfully for user {userId}.");

        return BusinessModuleMapper.ToGetProductResponse(product);
    }

    public async Task<ErrorOr<Updated>> UpdateProduct(string productId, UpdateProductRequest request)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        logger.LogInformation($"Received request to update product {productId} for user {userId}.\nRequest: {request}");
        var validateResult = await new UpdateProductRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            logger.LogError("Validation failed for UpdateProductRequest.");
            return validateResult.ToErrorList();
        }

        var product = await context.Products
            .Where(c => c.Business.UserId == userId && c.ProductId == productId).Include(product => product.Business)
            .FirstOrDefaultAsync();

        if (product is null)
        {
            logger.LogError($"Product {productId} not found for user {userId}.");
            return SharedErrors<Product>.NotFound;
        }

        if (product.Business.UserId != userId)
        {
            logger.LogError($"Invalid business for product {productId}.");
            return Errors.Product.InvalidBusiness;
        }

        product.Name = request.Name ?? product.Name;
        product.Description = request.Description ?? product.Description;
        product.Price = request.Price ?? product.Price;
        product.Quantity = request.Quantity ?? product.Quantity;
        product.ProductType = request.ProductType ?? product.ProductType;
        if (request.ProductImage is not null)
        {
            product.ImageUrl = await UploadImageToS3(request.ProductImage) ?? product.ImageUrl;
        }

        context.Update(product);
        await context.SaveChangesAsync();
        logger.LogInformation($"Product {productId} updated successfully for user {userId}.");
        return Result.Updated;
    }

    public async Task<ErrorOr<PagedResponse<GetProductResponse>>> GetAllProducts(GetProductsRequest request)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        Enum.TryParse<ProductSortOptions>(request.SortOption, out var sortOption);

        logger.LogInformation($"Received request to get all products for user {userId}.");

        var query = context.Products
            .Include(x => x.Business)
            .Where(x => x.Business.UserId == userId);

        query = ApplySearch(query, request);
        query = ApplyFilters(query, request);
        query = Sort(query, sortOption);

        var pagedResults = query.Select(x =>
            new GetProductResponse(
                x.ProductId,
                x.Name,
                x.Description,
                x.Price,
                x.Quantity,
                x.ImageUrl,
                x.ProductType));

        logger.LogInformation($"All products retrieved successfully for user {userId}.");

        return await new PagedResponse<GetProductResponse>().ToPagedList(pagedResults, request.PageNumber,
            request.PageSize);
    }

    private static IQueryable<Product> ApplySearch(IQueryable<Product> query, GetProductsRequest request)
    {
        return request.Search is not null ? query.Where(x => x.Name.Contains(request.Search)) : query;
    }

    public async Task<ErrorOr<Deleted>> DeleteProduct(string productId)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        logger.LogInformation($"Received request to delete product {productId} from user {userId}.");

        var product = await context.Products
            .Where(c => c.Business.UserId == userId && c.ProductId == productId)
            .FirstOrDefaultAsync();

        if (product is null)
        {
            logger.LogError($"Product {productId} not found for user {userId}.");
            return SharedErrors<Product>.NotFound;
        }

        context.Remove(product);
        await context.SaveChangesAsync();

        logger.LogInformation($"Product {productId} deleted successfully for user {userId}.");

        return Result.Deleted;
    }

    public async Task DeleteAllProductsRelatedToBusiness(string businessId)
    {
        logger.LogInformation($"Received request to delete all products related to business {businessId}.");

        var products = await context.Products
            .Where(x => x.BusinessId == businessId)
            .ToListAsync();

        context.RemoveRange(products);
        await context.SaveChangesAsync();

        logger.LogInformation($"All products related to business {businessId} deleted successfully.");
    }

    public async Task<ErrorOr<int>> GetTotalProductsForCurrentUserBusiness()
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        logger.LogInformation($"Received request to get total products for user {userId}.");

        var count = await context.Products
            .Where(c => c.Business.UserId == userId)
            .CountAsync();

        logger.LogInformation($"Total products for user {userId}: {count}.");
        return count;
    }

    private IQueryable<Product> ApplyFilters(IQueryable<Product> query, GetProductsRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ProductType))
        {
            return query;
        }

        Enum.TryParse<ProductType>(request.ProductType, out var productType);
        query = query.Where(
            x => x.ProductType.Equals(productType.ToString(), StringComparison.CurrentCultureIgnoreCase));

        logger.LogInformation($"Applied filters: ProductType = {request.ProductType}");

        return query;
    }

    private IQueryable<Product> Sort(IQueryable<Product> query, ProductSortOptions sortOption)
    {
        query = sortOption switch
        {
            ProductSortOptions.NameAsc => query.OrderBy(x => x.Name),
            ProductSortOptions.NameDesc => query.OrderByDescending(x => x.Name),
            ProductSortOptions.PriceAsc => query.OrderBy(x => x.Price),
            ProductSortOptions.PriceDesc => query.OrderByDescending(x => x.Price),
            ProductSortOptions.QuantityAsc => query.OrderBy(x => x.Quantity),
            ProductSortOptions.QuantityDesc => query.OrderByDescending(x => x.Quantity),
            ProductSortOptions.TypeAsc => query.OrderBy(x => x.ProductType),
            ProductSortOptions.TypeDesc => query.OrderByDescending(x => x.ProductType),

            _ => query.OrderBy(x => x.Name)
        };

        logger.LogInformation($"Sorted query by: {sortOption}");

        return query;
    }
}