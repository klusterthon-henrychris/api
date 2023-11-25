using System.ComponentModel.DataAnnotations;

namespace Kluster.Shared.Domain;

public class Wallet
{
    public Wallet()
    {
        WalletId = "WAL-" + BusinessId;
    }

    [Key]
    public string WalletId { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "NGN";
    public required string BusinessId { get; set; }
    public Business Business { get; set; } = null!;
}