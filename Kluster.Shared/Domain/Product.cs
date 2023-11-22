using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kluster.Shared.Domain
{
    public class Product
    {
        [Key, MaxLength(DomainConstants.MaxIdLength)]
        // todo - name according to business?
        public string ProductId { get; set; } = "P-" + Guid.NewGuid();

        [MaxLength(DomainConstants.MaxNameLength)]
        public required string Name { get; set; }

        [MaxLength(DomainConstants.MaxDescriptionLength)]
        public required string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")] public required decimal Price { get; set; }
        public required int Quantity { get; set; }

        [MaxLength(DomainConstants.MaxJsonLength)]
        public required string ImageUrl { get; set; } // upload to s3 or sumn

        /// <summary>
        /// Can be either physical or digital.
        /// </summary>
        [MaxLength(DomainConstants.MaxEnumLength)]
        public required string ProductType { get; set; }

        // navigation properties
        [MaxLength(DomainConstants.MaxIdLength)]
        public required string BusinessId { get; set; }

        public Business Business { get; set; } = null!;
    }
}