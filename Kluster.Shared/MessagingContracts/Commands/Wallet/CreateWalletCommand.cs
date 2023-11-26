namespace Kluster.Shared.MessagingContracts.Commands.Wallet;

public record CreateWalletCommand(string BusinessId, decimal Balance);