using ErrorOr;
using Kluster.Shared.Domain;

namespace Kluster.Shared.ServiceErrors;

public static class SharedErrors<T>
{
    public static Error NotFound => Error.NotFound(
        code: $"{nameof(T)}.NotFound",
        description: $"The {nameof(T)} does not exist.");

    public static Error MissingName => Error.Validation(
        code: $"{nameof(T)}.MissingName",
        description: $"The {nameof(T)} has no name.");

    public static Error InvalidName => Error.Validation(
        code: $"{nameof(T)}.InvalidName",
        description: $"{nameof(T)} name must be at least {DomainConstants.MinNameLength}" +
                     $" characters long and at most {DomainConstants.MaxNameLength} characters long.");

    public static Error MissingFirstName => Error.Validation(
        code: $"{nameof(T)}.MissingFirstName",
        description: "First name is required.");

    public static Error InvalidFirstName => Error.Validation(
        code: $"{nameof(T)}.InvalidFirstName",
        description: $"First name must be at least {DomainConstants.MinNameLength}" +
                     $" characters long and at most {DomainConstants.MaxNameLength} characters long.");

    public static Error InvalidLastName => Error.Validation(
        code: $"{nameof(T)}.InvalidLastName",
        description: $"Last name must be at least {DomainConstants.MinNameLength}" +
                     $" characters long and at most {DomainConstants.MaxNameLength} characters long.");

    public static Error MissingLastName => Error.Validation(
        code: $"{nameof(T)}.MissingLastName",
        description: "Last name is required.");

    public static Error MissingAddress => Error.Validation(
        code: $"{nameof(T)}.MissingAddress",
        description: $"The {nameof(T).ToLower()} has no address.");

    public static Error InvalidAddress => Error.Validation(
        code: $"{nameof(T)}.InvalidAddress",
        description:
        $"The {nameof(T).ToLower()} address must have at least {DomainConstants.MinAddressLength} characters" +
        $" and at most {DomainConstants.MaxAddressLength} characters.");

    public static Error MissingEmailAddress => Error.Validation(
        code: $"{nameof(T)}.MissingEmailAddress",
        description: "The email address is missing.");

    public static Error InvalidEmailAddress => Error.Validation(
        code: $"{nameof(T)}.InvalidEmailAddress",
        description: "The email address provided is invalid.");
}