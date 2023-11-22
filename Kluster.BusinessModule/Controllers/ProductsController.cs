using System.ComponentModel.DataAnnotations;
using Kluster.BusinessModule.DTOs.Requests.Products;
using Kluster.BusinessModule.Services.Contracts;
using Kluster.Shared.API;
using Microsoft.AspNetCore.Mvc;
using Kluster.Shared.Extensions;

namespace Kluster.BusinessModule.Controllers
{
    public class ProductsController(IProductService productService) : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateProduct([Required, FromForm] CreateProductRequest request)
        {
            var createProductResponse = await productService.CreateProductAsync(request);
            return createProductResponse.Match(
                productResponse => CreatedAtAction(nameof(GetProduct), routeValues: new { id = productResponse.Id },
                    createProductResponse.ToSuccessfulApiResponse()),
                ReturnErrorResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {
            var getProductResult = await productService.GetProduct(id);

            // If successful, return the event data in an ApiResponse.
            // If an error occurs, return an error response using the ReturnErrorResponse method.
            return getProductResult.Match(
                _ => Ok(getProductResult.ToSuccessfulApiResponse()),
                ReturnErrorResponse);
        }

        [HttpPut("{productId}/update")]
        public async Task<IActionResult> UpdateProduct(string productId, [Required, FromForm] UpdateProductRequest request)
        {
            var updateUserResult = await productService.UpdateProduct(productId, request);
            return updateUserResult.Match(_ => NoContent(), ReturnErrorResponse);
        }
        
        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts([FromQuery] GetProductsRequest request)
        {
            var getProductsResult = await productService.GetAllProducts(request);

            // If successful, return the event data in an ApiResponse.
            // If an error occurs, return an error response using the ReturnErrorResponse method.
            return getProductsResult.Match(
                _ => Ok(getProductsResult.ToSuccessfulApiResponse()),
                ReturnErrorResponse);
        }
    }
}