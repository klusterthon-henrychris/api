using FluentValidation;
using Kluster.BusinessModule.DTOs.Requests.Products;
using Kluster.BusinessModule.ServiceErrors;
using Kluster.BusinessModule.Validators.Helpers;
using Kluster.Shared.Constants;
using Kluster.Shared.Validators;

namespace Kluster.BusinessModule.Validators;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name).ValidateName();
        RuleFor(x => x.Description).ValidateBusinessDescription();
        RuleFor(x => x.ProductType).ValidateProductType();

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage(Errors.Product.InvalidPrice.Description)
            .WithErrorCode(Errors.Product.InvalidPrice.Code);
        
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(1)
            .WithMessage(Errors.Product.InvalidQuantity.Description)
            .WithErrorCode(Errors.Product.InvalidQuantity.Code);

        RuleFor(x => x.ProductImage)
            .NotEmpty();
    }
}