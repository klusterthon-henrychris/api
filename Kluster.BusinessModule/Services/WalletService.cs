using Kluster.BusinessModule.Data;
using Kluster.BusinessModule.ServiceErrors;
using Kluster.Shared.MessagingContracts.Commands.Wallet;
using Kluster.Shared.SharedContracts.BusinessModule;
using Microsoft.EntityFrameworkCore;

namespace Kluster.BusinessModule.Services;

public class WalletService(ILogger<WalletService> logger, BusinessModuleDbContext context) : IWalletService
{
    public async Task CreateWallet(CreateWalletRequest request)
    {
        if (await context.Wallets.AnyAsync(x => x.BusinessId == request.BusinessId))
        {
            logger.LogError(Errors.Business.WalletAlreadyCreated.Description);
            return;
        }

        var wallet = BusinessModuleMapper.ToWallet(request);
        await context.AddAsync(wallet);
        await context.SaveChangesAsync();
        logger.LogInformation($"Wallet created for business: {request.BusinessId}");
    }
}