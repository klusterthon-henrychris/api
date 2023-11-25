using Kluster.BusinessModule.Data;
using Kluster.BusinessModule.ServiceErrors;
using Kluster.Shared.MessagingContracts.Events.Wallet;
using Kluster.Shared.SharedContracts.BusinessModule;
using Microsoft.EntityFrameworkCore;

namespace Kluster.BusinessModule.Services;

public class WalletService(ILogger<WalletService> logger, BusinessModuleDbContext context) : IWalletService
{
    public async Task CreateWallet(CreateWalletEvent createWalletEvent)
    {
        if (await context.Wallets.AnyAsync(x => x.BusinessId == createWalletEvent.BusinessId))
        {
            logger.LogError(Errors.Business.WalletAlreadyCreated.Description);
            return;
        }

        var wallet = BusinessModuleMapper.ToWallet(createWalletEvent);
        await context.AddAsync(wallet);
        await context.SaveChangesAsync();
        logger.LogInformation($"Wallet created for business: {createWalletEvent.BusinessId}");
    }
}