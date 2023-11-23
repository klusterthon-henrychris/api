using FluentValidation;
using Kluster.Shared.DTOs.Requests.Product;
using Kluster.Shared.Validators;

namespace Kluster.BusinessModule.Validators.Helpers;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        When(x => x.Name is not null, () =>
            RuleFor(x => x.Name)!.ValidateName());

        When(x => x.Description is not null, () =>
            RuleFor(x => x.Description)!.ValidateBusinessDescription());

        When(x => x.ProductType is not null, () =>
            RuleFor(x => x.ProductType)!.ValidateProductType());

        When(x => x.Price is not null, () =>
            RuleFor(x => x.Price).ValidatePrice());

        When(x => x.Quantity is not null, () =>
            RuleFor(x => x.Quantity)!.ValidateQuantity());
    }
}