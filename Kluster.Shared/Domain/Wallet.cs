using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kluster.Shared.Domain;

public class Wallet
{
    public Wallet()
    {
        WalletId = "WAL-" + BusinessId;
    }

    [Key, MaxLength(DomainConstants.MaxIdLength)]
    public string WalletId { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal Balance { get; set; }
    [MaxLength(3)] public string Currency { get; set; } = "NGN";

    [MaxLength(DomainConstants.MaxIdLength)]
    public required string BusinessId { get; set; }

    public Business Business { get; set; } = null!;
}