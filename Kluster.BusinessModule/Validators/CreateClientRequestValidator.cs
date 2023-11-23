using FluentValidation;
using Kluster.Shared.DTOs.Requests.Client;
using Kluster.Shared.Validators;

namespace Kluster.BusinessModule.Validators;

public class CreateClientRequestValidator : AbstractValidator<CreateClientRequest>
{
    public CreateClientRequestValidator()
    {
        RuleFor(x => x).NotEmpty();

        RuleFor(x => x.FirstName).ValidateFirstName();
        RuleFor(x => x.LastName).ValidateLastName();
        RuleFor(x => x.EmailAddress).ValidateEmailAddress();
        RuleFor(x => x.Address).ValidateAddress();
    }
}