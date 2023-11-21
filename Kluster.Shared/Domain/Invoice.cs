using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kluster.Shared.Domain
{
    public class Invoice
    {
        [Key, MaxLength(DomainConstants.MaxIdLength)]
        // todo: for references and invoices, use a custom algorithm to set them.
        public string InvoiceNo { get; init; } = "I-" + Guid.NewGuid();

        [Column(TypeName = "decimal(18,2)")] public decimal Amount { get; set; }

        public DateTime DueDate { get; set; }
        public DateTime DateOfIssuance { get; set; }

        [MaxLength(DomainConstants.MaxEnumLength)]
        public required string Status { get; set; }

        /// <summary>
        /// add invoice items as JSON
        /// </summary>
        [MaxLength(DomainConstants.MaxJsonLength)]
        public string? InvoiceItems { get; set; }


        // navigation properties
        [MaxLength(DomainConstants.MaxIdLength)]
        public required string ClientId { get; set; }

        [MaxLength(DomainConstants.MaxIdLength)]
        public required string BusinessId { get; set; }

        public Client Client { get; set; } = null!;
        public Business Business { get; set; } = null!;
    }
}