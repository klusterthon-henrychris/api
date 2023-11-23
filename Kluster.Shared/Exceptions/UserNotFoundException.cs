namespace Kluster.Shared.Exceptions;

/// <summary>
/// Throws the message: The userId used for OTP generation is not linked to an existing user."
/// </summary>
/// <param name="message">Pass in a custom message or leave blank</param>
public class UserNotFoundException(string message = "") :
    Exception(string.IsNullOrEmpty(message)
        ? "The userId used for OTP generation is not linked to an existing user."
        : message
    );