using ErrorOr;

namespace Kluster.Shared.Middleware
{
    public static class SharedErrors
    {
        public static Error GenericError => Error.Unexpected(
        code: "Event.GenericError",
        description: "Sorry, something went wrong. Please reach out to an admin.");
    }
}
