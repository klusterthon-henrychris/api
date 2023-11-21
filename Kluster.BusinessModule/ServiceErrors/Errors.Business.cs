using ErrorOr;
using Kluster.Shared.Domain;

namespace Kluster.BusinessModule.ServiceErrors;

public static partial class Errors
{
    public static class Business
    {
        public static Error NotFound => Error.NotFound(
            code: $"{nameof(Business)}.NotFound",
            description: $"{nameof(Business)} not found.");

        public static Error MissingBusinessName => Error.Validation(
            code: $"{nameof(Business)}.MissingBusinessName",
            description: "The business has no name.");

        public static Error InvalidName => Error.Validation(
            code: $"{nameof(Business)}.InvalidName",
            description: $"{nameof(Business)} name must be at least {DomainConstants.MinNameLength}" +
                         $" characters long and at most {DomainConstants.MaxNameLength} characters long.");

        public static Error MissingBusinessAddress => Error.Validation(
            code: $"{nameof(Business)}.MissingBusinessAddress",
            description: "The business has no address.");

        public static Error InvalidBusinessAddress => Error.Validation(
            code: $"{nameof(Business)}.InvalidBusinessAddress",
            description: $"The business address must have at least {DomainConstants.MinAddressLength} characters" +
                         $" and at most {DomainConstants.MaxAddressLength} characters.");

        public static Error MissingRcNumber => Error.Validation(
            code: $"{nameof(Business)}.MissingRcNumber",
            description: "The business has no RcNumber.");

        public static Error InvalidRcNumber => Error.Validation(
            code: $"{nameof(Business)}.InvalidRcNumber",
            description:
            $"{nameof(Business)} name must have at most {DomainConstants.MaxRcNumberLength} characters.");

        public static Error MissingCacNumber => Error.Validation(
            code: $"{nameof(Business)}.MissingCacNumber",
            description: "The business has no CacNumber.");

        public static Error InvalidCacNumber => Error.Validation(
            code: $"{nameof(Business)}.InvalidCacNumber",
            description:
            $"{nameof(Business)} name must have at most {DomainConstants.MaxCacNumberLength} characters.");

        public static Error MissingIndustry => Error.Validation(
            code: $"{nameof(Business)}.MissingIndustry",
            description: "The business has no industry.");

        public static Error InvalidIndustry => Error.Validation(
            code: $"{nameof(Business)}.InvalidIndustry",
            description:
            $"{nameof(Business)} name must have at most {DomainConstants.MaxEnumLength} characters.");

        public static Error InvalidDescription => Error.Validation(
            code: $"{nameof(Business)}.InvalidDescription",
            description:
            $"{nameof(Business)} description must have at most {DomainConstants.MaxDescriptionLength} characters.");

        public static Error InvalidClientId => Error.Validation(
            code: $"{nameof(Business)}.InvalidClientId",
            description:
            "The business must be linked to a client.");
    }
}