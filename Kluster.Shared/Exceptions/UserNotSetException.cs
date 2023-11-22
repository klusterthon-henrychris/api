namespace Kluster.Shared.Exceptions;

/// <summary>
/// Throws the message: "There is no user in the JWT, or the JWT is invalid."
/// </summary>
/// <param name="message">Pass in a custom message or leave blank</param>
public class UserNotSetException(string message = "") :
    Exception(string.IsNullOrEmpty(message)
        ? "There is no user in the JWT, or the JWT is invalid."
        : message
    );