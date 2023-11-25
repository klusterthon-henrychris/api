using Kluster.BusinessModule.Data;
using Kluster.Shared.MessagingContracts.Commands.Wallet;
using Kluster.Shared.SharedContracts.BusinessModule;

namespace Kluster.BusinessModule.Services;

public class WalletService(ILogger<WalletService> logger, BusinessModuleDbContext context) : IWalletService
{
    public async Task CreateWallet(CreateWalletRequest request)
    {
        var wallet = BusinessModuleMapper.ToWallet(request);
        await context.AddAsync(wallet);
        await context.SaveChangesAsync();
        logger.LogInformation($"Wallet created for business: {request.BusinessId}");
    }
}