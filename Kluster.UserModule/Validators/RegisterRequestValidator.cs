using FluentValidation;
using Kluster.Shared.Constants;
using Kluster.Shared.Validators;
using Kluster.UserModule.DTOs.Requests;

namespace Kluster.UserModule.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x)
            .NotEmpty();

        RuleFor(x => x.FirstName).ValidateFirstName();
        RuleFor(x => x.LastName).ValidateLastName();
        RuleFor(x => x.EmailAddress).ValidateEmailAddress();
        RuleFor(x => x.Address).ValidateAddress();

        RuleFor(x => x.Role)
            .Must(x => UserRoles.AllRoles.Contains(x))
            .WithMessage("These are the valid roles: " + string.Join(", ", UserRoles.AllRoles));
    }
}