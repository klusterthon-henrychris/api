using System.ComponentModel.DataAnnotations;

namespace Kluster.Shared.Domain
{
    public class Business
    {
        // todo: use fluent validation to replace data annotations.
        // prepend 'B' to Id
        public string Id { get; set; } = "B-" + Guid.NewGuid().ToString();

        [MaxLength(50)]
        public required string Name { get; set; }
        public required string Address { get; set; }
        public string? CacNumber { get; set; }
        public string? RcNumber { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public required string Industry { get; set;}

        // navigation properties
        public required string UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public List<Invoice> Invoices { get; set; } = [];
        public List<Payment> Payments { get; set; } = [];
        public List<Product> Products { get; set; } = [];
    }
}
