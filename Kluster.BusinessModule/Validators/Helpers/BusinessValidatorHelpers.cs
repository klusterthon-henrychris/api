using FluentValidation;
using Kluster.BusinessModule.ServiceErrors;
using Kluster.Shared.Domain;

namespace Kluster.BusinessModule.Validators.Helpers;

public static class BusinessValidatorHelpers
{
    public static void ValidateRcNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty()
            .WithMessage(Errors.Business.MissingRcNumber.Description)
            .WithErrorCode(Errors.Business.MissingRcNumber.Code)
            .MaximumLength(DomainConstants.MaxRcNumberLength)
            .WithMessage(Errors.Business.InvalidRcNumber.Description)
            .WithErrorCode(Errors.Business.InvalidRcNumber.Code);
    }

    public static void ValidateIndustry<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty()
            .WithMessage(Errors.Business.MissingIndustry.Description)
            .WithErrorCode(Errors.Business.MissingIndustry.Code)
            .MaximumLength(DomainConstants.MaxEnumLength)
            .WithMessage(Errors.Business.InvalidIndustry.Description)
            .WithErrorCode(Errors.Business.InvalidIndustry.Code);
    }

    public static void ValidateBusinessDescription<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder
            .MaximumLength(DomainConstants.MaxDescriptionLength)
            .WithMessage(Errors.Business.InvalidDescription.Description)
            .WithErrorCode(Errors.Business.InvalidDescription.Code);
    }
}