using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kluster.Shared.Domain
{
    public class Payment
    {
        [Key, MaxLength(AppConstants.MaxIdLength)]
        public string PaymentReference { get; set; } = "Ref-" + Guid.NewGuid();
        [Column(TypeName = "decimal(18,2)")]
        public required decimal Amount { get; set; }
        public required DateTime DateOfPayment { get; set; }
        public string? OtherDetails { get; set; }
        public string? PaymentChannel { get; set; }

        // navigation properties
        [MaxLength(AppConstants.MaxIdLength)] public required string BusinessId { get; set; } // Business association
        [MaxLength(AppConstants.MaxIdLength)] public required string InvoiceId { get; set; } // Invoice association

        public Business Business { get; set; } = null!;
        public Invoice Invoice { get; set; } = null!;

    }
}
