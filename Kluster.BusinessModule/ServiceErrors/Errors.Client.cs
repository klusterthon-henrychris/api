using ErrorOr;

namespace Kluster.BusinessModule.ServiceErrors;

public static partial class Errors
{
    public static class Client
    {
        public static Error NotFound => Error.NotFound(
            code: "Client.NotFound",
            description: "Client not found.");

        public static Error InvalidClientId => Error.Validation(
            code: $"{nameof(Client)}.InvalidClientId",
            description:
            "The business must be linked to a client.");
    }
}