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

public class ProductService(ICurrentUser currentUser, BusinessModuleDbContext context) : IProductService
{
    public async Task<ErrorOr<CreateProductResponse>> CreateProductAsync(CreateProductRequest request)
    {
        var validateResult = await new CreateProductRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            return validateResult.ToErrorList();
        }

        var userId = currentUser.UserId ?? throw new UserNotSetException();
        var businessId = await context.Businesses
            .Where(x => x.UserId == userId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

        if (businessId is null)
        {
            return SharedErrors<Business>.NotFound;
        }

        var imageUrl = await UploadImageToS3(request.ProductImage);

        var product = BusinessModuleMapper.ToProduct(request, businessId, imageUrl!);
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
        return new CreateProductResponse(product.ProductId);
    }

    public Task<string?> UploadImageToS3(IFormFile requestProductImage)
    {
        // todo: find a place to upload images.
        // todo: remove nullable suppressor later
        return Task.FromResult(
            "https://fastly.picsum.photos/id/826/200/200.jpg?hmac=WlCuCjxEhXh_s4IkOpulPoB-LOoGjfZwP4GjNnkzTLA")!;
    }

    public async Task<ErrorOr<GetProductResponse>> GetProduct(string id)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();

        var product = await context.Products
            .Where(c => c.Business.UserId == userId && c.ProductId == id)
            .FirstOrDefaultAsync();

        return product is null ? SharedErrors<Product>.NotFound : BusinessModuleMapper.ToGetProductResponse(product);
    }

    public async Task<ErrorOr<Updated>> UpdateProduct(string productId, UpdateProductRequest request)
    {
        var validateResult = await new UpdateProductRequestValidator().ValidateAsync(request);
        if (!validateResult.IsValid)
        {
            return validateResult.ToErrorList();
        }

        var userId = currentUser.UserId ?? throw new UserNotSetException();
        var product = await context.Products
            .Where(c => c.Business.UserId == userId && c.ProductId == productId).Include(product => product.Business)
            .FirstOrDefaultAsync();

        if (product is null)
        {
            return SharedErrors<Product>.NotFound;
        }

        if (product.Business.UserId != userId)
        {
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
        return Result.Updated;
    }

    public Task<ErrorOr<PagedList<GetProductResponse>>> GetAllProducts(GetProductsRequest request)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();
        Enum.TryParse<ProductSortOptions>(request.SortOption, out var sortOption);

        var query = context.Products
            .Include(x => x.Business)
            .Where(x => x.Business.UserId == userId);

        query = ApplySearch(query, request);
        query = ApplyFilters(query, request);
        query = Sort(query, sortOption);

        var pagedResults = PagedList<GetProductResponse>
            .ToPagedList(query.Select(x =>
                    new GetProductResponse(
                        x.Name,
                        x.Description,
                        x.Price,
                        x.Quantity,
                        x.ImageUrl,
                        x.ProductType)),
                request.PageNumber,
                request.PageSize);

        return Task.FromResult<ErrorOr<PagedList<GetProductResponse>>>(pagedResults);
    }

    private static IQueryable<Product> ApplySearch(IQueryable<Product> query, GetProductsRequest request)
    {
        return request.Search is not null ? query.Where(x => x.Name.Contains(request.Search)) : query;
    }

    public async Task<ErrorOr<Deleted>> DeleteProduct(string productId)
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();

        var product = await context.Products
            .Where(c => c.Business.UserId == userId && c.ProductId == productId)
            .FirstOrDefaultAsync();

        if (product is null)
        {
            return SharedErrors<Product>.NotFound;
        }

        context.Remove(product);
        await context.SaveChangesAsync();
        return Result.Deleted;
    }

    public async Task DeleteAllProductsRelatedToBusiness(string businessId)
    {
        var products = await context.Products
            .Where(x => x.BusinessId == businessId)
            .ToListAsync();

        context.RemoveRange(products);
        await context.SaveChangesAsync();
    }

    public async Task<ErrorOr<int>> GetTotalProductsForCurrentUserBusiness()
    {
        var userId = currentUser.UserId ?? throw new UserNotSetException();

        return await context.Products
            .Where(c => c.Business.UserId == userId)
            .CountAsync();
    }

    private static IQueryable<Product> ApplyFilters(IQueryable<Product> query, GetProductsRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ProductType))
        {
            return query;
        }

        Enum.TryParse<ProductType>(request.ProductType, out var productType);
        query = query.Where(x => x.ProductType.Equals(productType.ToString(), StringComparison.CurrentCultureIgnoreCase));
        return query;
    }

    private static IQueryable<Product> Sort(IQueryable<Product> query, ProductSortOptions sortOption)
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

        return query;
    }
}