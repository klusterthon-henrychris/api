using ErrorOr;
using Kluster.Shared.Domain;

namespace Kluster.Shared.ServiceErrors;

public static class SharedErrors<T>
{
    public static Error NotFound => Error.NotFound(
        code: $"{typeof(T).Name}.NotFound",
        description: $"The {typeof(T).Name} does not exist.");

    public static Error MissingName => Error.Validation(
        code: $"{typeof(T).Name}.MissingName",
        description: $"The {typeof(T).Name} has no name.");

    public static Error InvalidName => Error.Validation(
        code: $"{typeof(T).Name}.InvalidName",
        description: $"{typeof(T).Name} name must be at least {DomainConstants.MinNameLength}" +
                     $" characters long and at most {DomainConstants.MaxNameLength} characters long.");

    public static Error MissingFirstName => Error.Validation(
        code: $"{typeof(T).Name}.MissingFirstName",
        description: "First name is required.");

    public static Error InvalidFirstName => Error.Validation(
        code: $"{typeof(T).Name}.InvalidFirstName",
        description: $"First name must be at least {DomainConstants.MinNameLength}" +
                     $" characters long and at most {DomainConstants.MaxNameLength} characters long.");

    public static Error InvalidLastName => Error.Validation(
        code: $"{typeof(T).Name}.InvalidLastName",
        description: $"Last name must be at least {DomainConstants.MinNameLength}" +
                     $" characters long and at most {DomainConstants.MaxNameLength} characters long.");

    public static Error MissingLastName => Error.Validation(
        code: $"{typeof(T).Name}.MissingLastName",
        description: "Last name is required.");

    public static Error MissingAddress => Error.Validation(
        code: $"{typeof(T).Name}.MissingAddress",
        description: $"The {typeof(T).Name.ToLower()} has no address.");

    public static Error InvalidAddress => Error.Validation(
        code: $"{typeof(T).Name}.InvalidAddress",
        description:
        $"The {typeof(T).Name.ToLower()} address must have at least {DomainConstants.MinAddressLength} characters" +
        $" and at most {DomainConstants.MaxAddressLength} characters.");

    public static Error MissingEmailAddress => Error.Validation(
        code: $"{typeof(T).Name}.MissingEmailAddress",
        description: "The email address is missing.");

    public static Error InvalidEmailAddress => Error.Validation(
        code: $"{typeof(T).Name}.InvalidEmailAddress",
        description: "The email address provided is invalid.");
}