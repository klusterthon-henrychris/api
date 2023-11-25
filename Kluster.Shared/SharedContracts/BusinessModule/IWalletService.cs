using Kluster.Shared.MessagingContracts.Events.Wallet;

namespace Kluster.Shared.SharedContracts.BusinessModule;

public interface IWalletService
{
    Task CreateWallet(CreateWalletEvent createWalletEvent);
}