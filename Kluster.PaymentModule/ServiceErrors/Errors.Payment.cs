using ErrorOr;

namespace Kluster.PaymentModule.ServiceErrors;

public static partial class Errors
{
    public static class Payment
    {
        public static Error NotValid => Error.Validation(
            code: $"{nameof(Invoice)}.NotValid",
            description: "Could not validate transaction.");

        public static Error AlreadyCompleted => Error.Validation(
            code: $"{nameof(Invoice)}.AlreadyCompleted",
            description: "This payment has already been completed.");
    }
}