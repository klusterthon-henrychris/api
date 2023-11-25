using Kluster.Shared.DTOs.Requests.Wallet;
using Kluster.Shared.MessagingContracts.Commands.Wallet;

namespace Kluster.Shared.SharedContracts.BusinessModule;

public interface IWalletService
{
    Task CreateWallet(CreateWalletCommand createWalletCommand);

    void CreditWallet(CreditWalletRequest request);
}