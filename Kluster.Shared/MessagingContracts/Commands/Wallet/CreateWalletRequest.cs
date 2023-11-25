namespace Kluster.Shared.MessagingContracts.Commands.Wallet;

public record CreateWalletRequest(string BusinessId, decimal Balance);