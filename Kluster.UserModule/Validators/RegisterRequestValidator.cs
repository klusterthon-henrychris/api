using FluentValidation;
using Kluster.Shared;
using Kluster.Shared.Domain;
using Kluster.Shared.Validators;
using Kluster.UserModule.DTOs.Requests;
using Kluster.UserModule.ServiceErrors;

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

        string[] validRoles = [UserRoles.Admin, UserRoles.User];
        RuleFor(x => x.Role)
            .Must(x => validRoles.Contains(x))
            .WithMessage("These are the valid roles: " + string.Join(", ", validRoles));
    }
}