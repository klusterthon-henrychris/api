using System.ComponentModel.DataAnnotations;

namespace Kluster.Shared.Domain
{
    public class Business
    {
        // todo: use fluent validation to replace data annotations.
        // prepend 'B' to Id and generate ID's internally
        [MaxLength(AppConstants.MaxIdLength)] public string Id { get; set; } = "B-" + Guid.NewGuid();

        [MaxLength(BusinessConstants.MaxNameLength)]
        public required string Name { get; set; }

        [MaxLength(BusinessConstants.MaxAddressLength)]
        public required string Address { get; set; }

        [MaxLength(BusinessConstants.MaxCacNumberLength)]
        public string? CacNumber { get; set; }

        [MaxLength(BusinessConstants.MaxRcNumberLength)]
        public string? RcNumber { get; set; }

        [MaxLength(BusinessConstants.MaxDescriptionLength)]
        public string? Description { get; set; }

        [MaxLength(BusinessConstants.MaxIndustryLength)]
        public required string Industry { get; set; }

        // navigation properties
        [MaxLength(AppConstants.MaxIdLength)] public required string UserId { get; set; }

        public ApplicationUser User { get; set; } = null!;
    }

    public static class BusinessConstants
    {
        public const int MaxCacNumberLength = 15;
        public const int MaxRcNumberLength = 15;
        public const int MaxAddressLength = 50;
        public const int MaxIndustryLength = 20;
        public const int MaxDescriptionLength = 200;
        public const int MinNameLength = 3;
        public const int MaxNameLength = 50;
        public const int MinAddressLength = 3;
    }
}