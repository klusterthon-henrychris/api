using FluentValidation;
using Kluster.BusinessModule.Validators.Helpers;
using Kluster.Shared.DTOs.Requests.Business;
using Kluster.Shared.Validators;

namespace Kluster.BusinessModule.Validators;

public class UpdateBusinessRequestValidator : AbstractValidator<UpdateBusinessRequest>
{
    public UpdateBusinessRequestValidator()
    {
        When(x => x.Name is not null, () =>
            RuleFor(x => x.Name)!.ValidateName());

        When(x => x.Address is not null, () =>
            RuleFor(x => x.Address)!.ValidateAddress());
        
        When(x => x.RcNumber is not null, () =>
            RuleFor(x => x.RcNumber)!.ValidateRcNumber());

        When(x => x.Description is not null, () =>
            RuleFor(x => x.Description)!.ValidateBusinessDescription());

        When(x => x.Industry is not null, () =>
            RuleFor(x => x.Industry)!.ValidateIndustry());
    }
}