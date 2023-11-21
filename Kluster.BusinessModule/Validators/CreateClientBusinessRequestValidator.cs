using FluentValidation;
using Kluster.BusinessModule.DTOs.Requests;
using Kluster.BusinessModule.ServiceErrors;
using Kluster.Shared.Domain;

namespace Kluster.BusinessModule.Validators;

public class CreateClientBusinessRequestValidator : AbstractValidator<CreateClientBusinessRequest>
{
    public CreateClientBusinessRequestValidator()
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

        RuleFor(x => x.Industry)
            .NotEmpty()
            .WithMessage(Errors.Business.MissingIndustry.Description)
            .WithErrorCode(Errors.Business.MissingIndustry.Code)
            .MaximumLength(BusinessConstants.MaxIndustryLength)
            .WithMessage(Errors.Business.InvalidIndustry.Description)
            .WithErrorCode(Errors.Business.InvalidIndustry.Code);

        RuleFor(x => x.ClientId)
            .NotEmpty()
            .Must(IsGuid)
            .WithMessage(Errors.Business.InvalidClientId.Description)
            .WithErrorCode(Errors.Business.InvalidClientId.Code);
    }

    private bool IsGuid(string bar)
    {
        return Guid.TryParse(bar, out _);
    }
}