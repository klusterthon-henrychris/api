using System.ComponentModel.DataAnnotations;

namespace Kluster.Shared.Domain
{
    public class Business
    {
        // todo: use fluent validation to replace data annotations.
        // prepend 'B' to Id and generate ID's internally
        [MaxLength(DomainConstants.MaxIdLength)]
        public string Id { get; set; } = "B-" + Guid.NewGuid();

        [MaxLength(DomainConstants.MaxNameLength)]
        public required string Name { get; set; }

        [MaxLength(DomainConstants.MaxAddressLength)]
        public required string Address { get; set; }

        [MaxLength(DomainConstants.MaxCacNumberLength)]
        public string? CacNumber { get; set; }

        [MaxLength(DomainConstants.MaxRcNumberLength)]
        public string? RcNumber { get; set; }

        [MaxLength(DomainConstants.MaxDescriptionLength)]
        public string? Description { get; set; }

        [MaxLength(DomainConstants.MaxEnumLength)]
        public required string Industry { get; set; }

        // navigation properties
        [MaxLength(DomainConstants.MaxIdLength)]
        public required string UserId { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public List<Product> Products { get; set; } = [];
        public List<Client> Clients { get; set; } = [];
    }
}