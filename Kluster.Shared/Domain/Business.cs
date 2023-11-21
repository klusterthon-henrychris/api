using System.ComponentModel.DataAnnotations;

namespace Kluster.Shared.Domain
{
    public class Business
    {
        // todo: use fluent validation to replace data annotations.
        // prepend 'B' to Id
        [MaxLength(AppConstants.MaxIdLength)]
        public string Id { get; set; } = "B-" + Guid.NewGuid();

        [MaxLength(50)] public required string Name { get; set; }
        [MaxLength(50)] public required string Address { get; set; }
        [MaxLength(15)] public string? CacNumber { get; set; }
        [MaxLength(15)] public string? RcNumber { get; set; }

        [MaxLength(200)] public string? Description { get; set; }

        [MaxLength(20)] public required string Industry { get; set; }

        // navigation properties
        [MaxLength(AppConstants.MaxIdLength)] public required string UserId { get; set; }

        public ApplicationUser User { get; set; } = null!;
    }
}