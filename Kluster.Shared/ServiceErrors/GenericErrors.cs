using ErrorOr;

namespace Kluster.Shared.ServiceErrors
{
    public static class GenericErrors
    {
        public static Error GenericError => Error.Unexpected(
            code: "Event.GenericError",
            description: "Sorry, something went wrong. Please reach out to an admin.");
    }
}