using ErrorOr;

namespace Kluster.BusinessModule.ServiceErrors;

public static partial class Errors
{
    public static class Client
    {
        public static Error InvalidClientId => Error.Validation(
            code: $"{nameof(Client)}.InvalidClientId",
            description:
            "The business must be linked to a client.");
        
        public static Error InvalidBusiness => Error.Unexpected(
            code: $"{nameof(Client)}.InvalidBusiness",
            description:
            "This client is not linked to your business.");
    }
}