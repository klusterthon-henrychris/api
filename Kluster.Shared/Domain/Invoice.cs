using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kluster.Shared.Domain
{
    public class Invoice
    {
        [Key, MaxLength(DomainConstants.MaxIdLength)]
        public string InvoiceNo { get; init; } = SharedLogic.GenerateReference("INV");

        [Column(TypeName = "decimal(18,2)")] public required decimal Amount { get; set; }
        
        public required DateTime DueDate { get; set; }
        
        /// <summary>
        /// Set when creating invoice object.
        /// </summary>
        public required DateTime DateOfIssuance { get; set; }

        /// <summary>
        /// Set when creating invoice. Default is Due.
        /// </summary>
        [MaxLength(DomainConstants.MaxEnumLength)]
        public required string Status { get; set; }

        /// <summary>
        /// add invoice items as JSON
        /// </summary>
        [MaxLength(DomainConstants.MaxJsonLength)]
        public required string InvoiceItems { get; set; }

        [MaxLength(DomainConstants.MaxAddressLength)]
        public required string BillingAddress { get; set; }

        // navigation properties
        [MaxLength(DomainConstants.MaxIdLength)]
        public required string ClientId { get; set; }

        [MaxLength(DomainConstants.MaxIdLength)]
        public required string BusinessId { get; set; }

        public Client Client { get; set; } = null!;
        public Business Business { get; set; } = null!;
    }
}