using ErrorOr;

namespace Kluster.BusinessModule.ServiceErrors;

public static partial class Errors
{
    public static class Product
    {
        public static Error InvalidPrice => Error.Validation(
            code: $"{nameof(Product)}.InvalidPrice",
            description: "The price must be greater than, or equal to zero.");
        
        public static Error InvalidQuantity => Error.Validation(
            code: $"{nameof(Product)}.InvalidQuantity",
            description: "The quantity must be greater than, or equal to one.");
    }
}