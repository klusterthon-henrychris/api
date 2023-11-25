using Kluster.Shared.MessagingContracts.Commands.Wallet;

namespace Kluster.Shared.SharedContracts.BusinessModule;

public interface IWalletService
{
    Task CreateWallet(CreateWalletRequest request);
}