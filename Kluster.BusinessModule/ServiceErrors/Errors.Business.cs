using ErrorOr;
using Kluster.Shared.Domain;

namespace Kluster.BusinessModule.ServiceErrors;

public static partial class Errors
{
    public static class Business
    {
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
    }
}