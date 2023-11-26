using FluentValidation;
using Kluster.BusinessModule.Validators.Helpers;
using Kluster.Shared.DTOs.Requests.Business;
using Kluster.Shared.Validators;

namespace Kluster.BusinessModule.Validators;

public class CreateBusinessRequestValidator : AbstractValidator<CreateBusinessRequest>
{
    public CreateBusinessRequestValidator()
    {
        RuleFor(x => x).NotEmpty();
        RuleFor(x => x.BusinessName).ValidateName();
        RuleFor(x => x.BusinessAddress).ValidateAddress();
        RuleFor(x => x.RcNumber).ValidateRcNumber();
        RuleFor(x => x.Industry).ValidateIndustry();
        RuleFor(x => x.BusinessDescription).ValidateBusinessDescription();
    }
}