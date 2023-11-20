using System.ComponentModel.DataAnnotations;

namespace Kluster.Shared.Domain
{
    public class Payment
    {
        [Key]
        public string PaymentReference { get; set; } = "Ref-" + Guid.NewGuid().ToString();
        public required decimal Amount { get; set; }
        public required DateTime DateOfPayment { get; set; }
        public string? OtherDetails { get; set; }
        public string? PaymentChannel { get; set; }

        // navigation properties
        public required string BusinessId { get; set; } // Business association
        public required string InvoiceId { get; set; } // Invoice association

        public Business Business { get; set; } = null!;
        public Invoice Invoice { get; set; } = null!;

    }
}
