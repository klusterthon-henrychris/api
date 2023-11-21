using FluentValidation;
using Kluster.BusinessModule.DTOs.Requests;
using Kluster.BusinessModule.ServiceErrors;
using Kluster.Shared.Domain;

namespace Kluster.BusinessModule.Validators;

public class CreateBusinessRequestValidator : AbstractValidator<CreateBusinessRequest>
{
    public CreateBusinessRequestValidator()
    {
        RuleFor(x => x).NotEmpty();

        RuleFor(x => x.BusinessName)
            .NotEmpty()
            .WithMessage(Errors.Business.MissingBusinessName.Description)
            .WithErrorCode(Errors.Business.MissingBusinessName.Code)
            .Length(BusinessConstants.MinNameLength, BusinessConstants.MaxNameLength)
            .WithMessage(Errors.Business.InvalidName.Description)
            .WithErrorCode(Errors.Business.InvalidName.Code);


        RuleFor(x => x.BusinessAddress)
            .NotEmpty()
            .WithMessage(Errors.Business.MissingBusinessAddress.Description)
            .WithErrorCode(Errors.Business.MissingBusinessAddress.Code)
            .MaximumLength(BusinessConstants.MaxAddressLength)
            .WithMessage(Errors.Business.InvalidBusinessAddress.Description)
            .WithErrorCode(Errors.Business.InvalidBusinessAddress.Code);

        RuleFor(x => x.RcNumber)
            .NotEmpty()
            .WithMessage(Errors.Business.MissingRcNumber.Description)
            .WithErrorCode(Errors.Business.MissingRcNumber.Code)
            .MaximumLength(BusinessConstants.MaxRcNumberLength)
            .WithMessage(Errors.Business.InvalidRcNumber.Description)
            .WithErrorCode(Errors.Business.InvalidRcNumber.Code);

        RuleFor(x => x.CacNumber)
            .NotEmpty()
            .WithMessage(Errors.Business.MissingCacNumber.Description)
            .WithErrorCode(Errors.Business.MissingCacNumber.Code)
            .MaximumLength(BusinessConstants.MaxCacNumberLength)
            .WithMessage(Errors.Business.InvalidCacNumber.Description)
            .WithErrorCode(Errors.Business.InvalidCacNumber.Code);

        RuleFor(x => x.Industry)
            .NotEmpty()
            .WithMessage(Errors.Business.MissingIndustry.Description)
            .WithErrorCode(Errors.Business.MissingIndustry.Code)
            .MaximumLength(BusinessConstants.MaxIndustryLength)
            .WithMessage(Errors.Business.InvalidIndustry.Description)
            .WithErrorCode(Errors.Business.InvalidIndustry.Code);

        RuleFor(x => x.BusinessDescription)
            .MaximumLength(BusinessConstants.MaxDescriptionLength)
            .WithMessage(Errors.Business.InvalidDescription.Description)
            .WithErrorCode(Errors.Business.InvalidDescription.Code);
    }
}