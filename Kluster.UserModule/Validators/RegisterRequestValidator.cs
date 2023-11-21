using FluentValidation;
using Kluster.Shared;
using Kluster.Shared.Domain;
using Kluster.UserModule.DTOs.Requests;
using Kluster.UserModule.ServiceErrors;

namespace Kluster.UserModule.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x)
            .NotEmpty();

        RuleFor(x => x.FirstName).NotEmpty().Length(DomainConstants.MinNameLength, DomainConstants.MaxNameLength)
            .WithErrorCode(Errors.User.InvalidFirstName.Code)
            .WithMessage(Errors.User.InvalidFirstName.Description);

        RuleFor(x => x.LastName).NotEmpty().Length(DomainConstants.MinNameLength, DomainConstants.MaxNameLength)
            .WithErrorCode(Errors.User.InvalidLastName.Code)
            .WithMessage(Errors.User.InvalidLastName.Description);

        RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress()
            .WithErrorCode(Errors.User.InvalidEmailAddress.Code)
            .WithMessage(Errors.User.InvalidEmailAddress.Description);

        RuleFor(x => x.Address).NotEmpty()
            .MinimumLength(5)
            .WithErrorCode(Errors.User.MissingAddress.Code)
            .WithMessage(Errors.User.MissingAddress.Description);

        string[] validRoles = [UserRoles.Admin, UserRoles.User];
        RuleFor(x => x.Role)
            .Must(x => validRoles.Contains(x))
            .WithMessage("These are the valid roles: " + string.Join(", ", validRoles));
    }
}