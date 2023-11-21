using FluentValidation;
using Kluster.BusinessModule.DTOs.Requests;
using Kluster.BusinessModule.ServiceErrors;
using Kluster.Shared.Domain;
using Kluster.Shared.Validators;

namespace Kluster.BusinessModule.Validators;

public class CreateBusinessRequestValidator : AbstractValidator<CreateBusinessRequest>
{
    public CreateBusinessRequestValidator()
    {
        RuleFor(x => x).NotEmpty();
        RuleFor(x => x.BusinessName).ValidateName();
        RuleFor(x => x.BusinessAddress).ValidateAddress();

        RuleFor(x => x.RcNumber)
            .NotEmpty()
            .WithMessage(Errors.Business.MissingRcNumber.Description)
            .WithErrorCode(Errors.Business.MissingRcNumber.Code)
            .MaximumLength(DomainConstants.MaxRcNumberLength)
            .WithMessage(Errors.Business.InvalidRcNumber.Description)
            .WithErrorCode(Errors.Business.InvalidRcNumber.Code);

        RuleFor(x => x.CacNumber)
            .NotEmpty()
            .WithMessage(Errors.Business.MissingCacNumber.Description)
            .WithErrorCode(Errors.Business.MissingCacNumber.Code)
            .MaximumLength(DomainConstants.MaxCacNumberLength)
            .WithMessage(Errors.Business.InvalidCacNumber.Description)
            .WithErrorCode(Errors.Business.InvalidCacNumber.Code);

        RuleFor(x => x.Industry)
            .NotEmpty()
            .WithMessage(Errors.Business.MissingIndustry.Description)
            .WithErrorCode(Errors.Business.MissingIndustry.Code)
            .MaximumLength(DomainConstants.MaxEnumLength)
            .WithMessage(Errors.Business.InvalidIndustry.Description)
            .WithErrorCode(Errors.Business.InvalidIndustry.Code);

        RuleFor(x => x.BusinessDescription)
            .MaximumLength(DomainConstants.MaxDescriptionLength)
            .WithMessage(Errors.Business.InvalidDescription.Description)
            .WithErrorCode(Errors.Business.InvalidDescription.Code);
    }
}