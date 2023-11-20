using System.ComponentModel.DataAnnotations.Schema;

namespace Kluster.Shared.Domain
{
    public class Invoice
    {
        public string InvoiceNo { get; set; } = "I-" + Guid.NewGuid().ToString();

        public decimal Amount { get; set; }

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
        
        [ForeignKey("Payment")]
        public string? PaymentReference { get; set; }

        public ApplicationUser Client { get; set; } = null!;
        public Business Business { get; set; } = null!;
        public Payment? Payment { get; set; }
    }
}
