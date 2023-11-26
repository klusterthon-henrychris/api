namespace Kluster.Shared.DTOs.Requests.Wallet;

public record GetWalletBalanceResponse(string Id, string BusinessName, decimal Balance);