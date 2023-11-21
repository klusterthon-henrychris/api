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
    }
}