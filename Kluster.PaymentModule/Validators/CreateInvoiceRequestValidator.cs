using FluentValidation;
using Kluster.PaymentModule.ServiceErrors;
using Kluster.Shared.DTOs.Requests.Invoices;
using Kluster.Shared.Validators;

namespace Kluster.PaymentModule.Validators;

public class CreateInvoiceRequestValidator : AbstractValidator<CreateInvoiceRequest>
{
    public CreateInvoiceRequestValidator()
    {
        RuleFor(x => x.Amount)
            .NotEmpty()
            .GreaterThanOrEqualTo(0)
            .WithMessage(Errors.Invoice.InvalidAmount.Description)
            .WithErrorCode(Errors.Invoice.InvalidAmount.Code);
        
        RuleFor(x => x.DueDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.Now)
            .WithMessage(Errors.Invoice.InvalidDueDate.Description)
            .WithErrorCode(Errors.Invoice.InvalidDueDate.Code);
        
        RuleFor(x => x.InvoiceItems)
            .NotEmpty()
            .WithMessage(Errors.Invoice.NoItemsInInvoice.Description)
            .WithErrorCode(Errors.Invoice.NoItemsInInvoice.Code);
        
        RuleFor(x => x.BillingAddress)
            .ValidateAddress();
        
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithMessage(Errors.Invoice.ClientNotSelected.Description)
            .WithErrorCode(Errors.Invoice.ClientNotSelected.Code);
    }
}