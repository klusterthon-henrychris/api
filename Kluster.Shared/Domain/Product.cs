using System.ComponentModel.DataAnnotations;

namespace Kluster.Shared.Domain
{
    public class Product
    {
        [Key]
        public string ProductId { get; set; } = "P-" + Guid.NewGuid().ToString();

        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required int Quantity { get; set; }
        public required string ImageUrl { get; set; } // upload to s3 or sumn
        
        /// <summary>
        /// Can be either physical or digital.
        /// </summary>
        public required string ProductType { get; set; } 

        // navigation properties
        public required string BusinessId { get; set; }
        public Business Business { get; set; } = null!;
    }
}
