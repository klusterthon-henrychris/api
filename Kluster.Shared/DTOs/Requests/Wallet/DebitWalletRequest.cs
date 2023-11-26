namespace Kluster.Shared.DTOs.Requests.Wallet;

public record DebitWalletRequest(string BusinessId, decimal Amount);