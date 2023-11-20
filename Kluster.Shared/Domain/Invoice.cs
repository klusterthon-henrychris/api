using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kluster.Shared.Domain
{
    public class Invoice
    {
        [Key] public string InvoiceNo { get; set; } = "I-" + Guid.NewGuid();
        [Column(TypeName = "decimal(18,2)")] public decimal Amount { get; set; }

        public DateTime DueDate { get; set; }
        public DateTime DateOfIssuance { get; set; }

        public required string Status { get; set; }

        /// <summary>
        /// add invoice items as JSON
        /// </summary>
        public string? InvoiceItems { get; set; }


        // navigation properties
        public required string ClientId { get; set; }
        public required string BusinessId { get; set; }
        public ApplicationUser Client { get; set; } = null!;
        public Business Business { get; set; } = null!;
    }
}