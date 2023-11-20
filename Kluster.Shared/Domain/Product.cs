using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kluster.Shared.Domain
{
    public class Product
    {
        [Key, MaxLength(30)] public string ProductId { get; set; } = "P-" + Guid.NewGuid();

        public required string Name { get; set; }
        public required string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")] public required decimal Price { get; set; }
        public required int Quantity { get; set; }
        public required string ImageUrl { get; set; } // upload to s3 or sumn

        /// <summary>
        /// Can be either physical or digital.
        /// </summary>
        public required string ProductType { get; set; }

        // navigation properties
        [MaxLength(30)] public required string BusinessId { get; set; }
        public Business Business { get; set; } = null!;
    }
}