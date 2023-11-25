namespace Kluster.Shared.MessagingContracts.Events.Wallet;

public record CreateWalletEvent(string BusinessId, decimal Balance);