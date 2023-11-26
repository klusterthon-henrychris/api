using ErrorOr;

namespace Kluster.PaymentModule.ServiceErrors;

public static partial class Errors
{
    public static class Invoice
    {
        public static Error InvalidAmount => Error.Validation(
            code: $"{nameof(Invoice)}.InvalidAmount",
            description: "The total amount must be greater than, or equal to zero.");

        public static Error InvalidDueDate => Error.Validation(
            code: $"{nameof(Invoice)}.InvalidDueDate",
            description: "The invoice must be due some time in the future.");

        public static Error NoItemsInInvoice => Error.Validation(
            code: $"{nameof(Invoice)}.NoItemsInInvoice",
            description: "No items have been added to this invoice.");


        public static Error ClientNotSelected => Error.Validation(
            code: $"{nameof(Invoice)}.ClientNotSelected",
            description: "No client has been selected for this invoice.");
        
        public static Error PaymentAlreadyCompleted => Error.Validation(
            code: $"{nameof(Invoice)}.PaymentAlreadyCompleted",
            description: "This invoice has already been paid.");
    }
}