using FluentValidation;
using Kluster.BusinessModule.ServiceErrors;
using Kluster.Shared.Constants;

namespace Kluster.BusinessModule.Validators.Helpers;

public static class ProductValidatorHelper
{
    public static void ValidateQuantity<T>(this IRuleBuilder<T, int?> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty()
            .GreaterThanOrEqualTo(1)
            .WithMessage(Errors.Product.InvalidQuantity.Description)
            .WithErrorCode(Errors.Product.InvalidQuantity.Code);
    }

    public static void ValidatePrice<T>(this IRuleBuilder<T, decimal?> ruleBuilder)
    {
        ruleBuilder
            .GreaterThanOrEqualTo(0)
            .WithMessage(Errors.Product.InvalidPrice.Description)
            .WithErrorCode(Errors.Product.InvalidPrice.Code);
    }

    public static void ValidateProductType<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder
            .Must(x => ProductTypeStrings.AllProductTypeOptions.Contains(x))
            .WithMessage("These are the valid product classification: " +
                         string.Join(", ", ProductTypeStrings.AllProductTypeOptions));
    }
}