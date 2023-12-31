﻿using ErrorOr;
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
        
        public static Error BusinessAlreadyExists => Error.Unexpected(
            code: $"{nameof(Business)}.BusinessAlreadyExists",
            description: "The user has already created a business.");
        
        public static Error WalletNotCreated => Error.Unexpected(
            code: $"{nameof(Business)}.WalletNotCreated",
            description: "The user does not have a wallet.");
        
        public static Error WalletAlreadyCreated => Error.Unexpected(
            code: $"{nameof(Business)}.WalletNotCreated",
            description: "The user already has a wallet.");
    }
}