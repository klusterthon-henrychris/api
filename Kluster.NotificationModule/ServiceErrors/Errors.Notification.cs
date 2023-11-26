using ErrorOr;

namespace Kluster.NotificationModule.ServiceErrors;

public static class Errors
{
    public static class Notification
    {
        public static Error ForgotPasswordEmailFailed => Error.Failure(
            code: $"{nameof(Notification)}.ForgotPasswordEmailFailed",
            description: "Failed to send forgot password email.");
        
        public static Error InitialInvoiceEmailFailed => Error.Failure(
            code: $"{nameof(Notification)}.InitialInvoiceEmailFailed",
            description: "Failed to send initial invoice email.");
    }
}