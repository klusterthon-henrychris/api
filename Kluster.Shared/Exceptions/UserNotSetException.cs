namespace Kluster.Shared.Exceptions;

public class UserNotSetException(string? message) :
    Exception(string.IsNullOrEmpty(message)
        ? "There is no user in the JWT, or the JWT is invalid."
        : message
    );