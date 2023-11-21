using ErrorOr;

namespace Kluster.BusinessModule.ServiceErrors;

public static partial class Errors
{
    public static class Client
    {
        public static Error NotFound => Error.NotFound(
            code: "Client.NotFound",
            description: "Client not found.");
    }
}