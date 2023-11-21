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
        RuleFor(x => x.FirstName).NotEmpty().Length(AppConstants.MinNameLength, AppConstants.MaxNameLength)
            .WithErrorCode(Errors.User.InvalidFirstName.Code)
            .WithMessage(Errors.User.InvalidFirstName.Description);
        RuleFor(x => x.LastName).NotEmpty().Length(AppConstants.MinNameLength, AppConstants.MaxNameLength)
            .WithErrorCode(Errors.User.InvalidLastName.Code)
            .WithMessage(Errors.User.InvalidLastName.Description);
        RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress()
            .WithErrorCode(Errors.User.InvalidEmailAddress.Code)
            .WithMessage(Errors.User.InvalidEmailAddress.Description);

        string[] validRoles = [UserRoles.Client, UserRoles.Business];
        RuleFor(x => x.Role)
            .Must(x => validRoles.Contains(x))
            .WithMessage("These are the valid roles: " + string.Join(", ", validRoles));
    }
}