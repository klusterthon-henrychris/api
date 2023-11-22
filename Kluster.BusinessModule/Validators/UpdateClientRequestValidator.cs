using FluentValidation;
using Kluster.BusinessModule.DTOs.Requests;
using Kluster.Shared.Validators;

namespace Kluster.BusinessModule.Validators;

public class UpdateClientRequestValidator : AbstractValidator<UpdateClientRequest>
{
    public UpdateClientRequestValidator()
    {
        When(x => x.FirstName is not null, () =>
            RuleFor(x => x.FirstName)!.ValidateFirstName());
        When(x => x.LastName is not null, () =>
            RuleFor(x => x.LastName)!.ValidateLastName());
        When(x => x.Address is not null, () =>
            RuleFor(x => x.Address)!.ValidateAddress());
        When(x => x.EmailAddress is not null, () =>
            RuleFor(x => x.EmailAddress)!.ValidateEmailAddress());
        When(x => x.BusinessName is not null, () =>
            RuleFor(x => x.BusinessName)!.ValidateName());
    }
}