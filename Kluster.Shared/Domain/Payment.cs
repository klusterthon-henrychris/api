using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kluster.Shared.Domain
{
    public class Payment
    {
        [Key, MaxLength(DomainConstants.MaxIdLength)]
        public string PaymentReference { get; set; } = "Ref-" + Guid.NewGuid();

        [Column(TypeName = "decimal(18,2)")] public required decimal Amount { get; set; }
        public required DateTime DateOfPayment { get; set; }

        [MaxLength(DomainConstants.MaxJsonLength)]
        public string? OtherDetails { get; set; }

        [MaxLength(DomainConstants.MaxEnumLength)] public string? PaymentChannel { get; set; }

        // navigation properties
        [MaxLength(DomainConstants.MaxIdLength)]
        public required string BusinessId { get; set; }

        [MaxLength(DomainConstants.MaxIdLength)]
        public required string InvoiceId { get; set; }

        [MaxLength(DomainConstants.MaxIdLength)]
        public required string ClientId { get; set; }

        public Business Business { get; set; } = null!;
        public Invoice Invoice { get; set; } = null!;
        public Client Client { get; set; } = null!;
    }
}