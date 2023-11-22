using FluentValidation;
using Kluster.Shared.Validators;
using Kluster.UserModule.DTOs.Requests;

namespace Kluster.UserModule.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        When(x => x.FirstName is not null, () =>
            RuleFor(x => x.FirstName)!.ValidateFirstName());

        When(x => x.LastName is not null, () =>
            RuleFor(x => x.LastName)!.ValidateLastName());

        When(x => x.Address is not null, () =>
            RuleFor(x => x.Address)!.ValidateAddress());
    }
}