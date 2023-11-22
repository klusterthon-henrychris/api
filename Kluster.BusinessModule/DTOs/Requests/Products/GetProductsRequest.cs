using Kluster.BusinessModule.Constants;
using Kluster.Shared.Requests;

namespace Kluster.BusinessModule.DTOs.Requests.Products;

// todo: add search query param to getAll requests.
// If search param exists, skip straight to sorting
// and filtering the results of the search.
// only use for products with ONE standout name.

// remove some filters from get clients. they aren't actually filters.
public class GetProductsRequest : QueryStringParameters
{
    // todo: add name search
    public string? ProductType { get; set; }
    public string? SortOption { get; set; } = ProductSortStrings.NameAsc;
}