using Kluster.BusinessModule.Data;
using Kluster.BusinessModule.ServiceErrors;
using Kluster.Shared.DTOs.Requests.Wallet;
using Kluster.Shared.MessagingContracts.Commands.Wallet;
using Kluster.Shared.SharedContracts.BusinessModule;
using Microsoft.EntityFrameworkCore;

namespace Kluster.BusinessModule.Services;

public class WalletService(ILogger<WalletService> logger, BusinessModuleDbContext context) : IWalletService
{
    private object _creditLock = new { };

    public async Task CreateWallet(CreateWalletCommand createWalletCommand)
    {
        if (await context.Wallets.AnyAsync(x => x.BusinessId == createWalletCommand.BusinessId))
        {
            logger.LogError(Errors.Business.WalletAlreadyCreated.Description);
            return;
        }

        var wallet = BusinessModuleMapper.ToWallet(createWalletCommand);
        await context.AddAsync(wallet);
        await context.SaveChangesAsync();
        logger.LogInformation($"Wallet created for business: {createWalletCommand.BusinessId}");
    }

    public void CreditWallet(CreditWalletRequest request)
    {
        // todo: enqueue the method that calls this, so that it can retry in case of exception.
        // the caller method is that in the webhook that verifies the transaction, etc, etc.
        lock (_creditLock)
        {
            var wallet = context.Wallets.FirstOrDefault(x => x.BusinessId == request.BusinessId);
            if (wallet is null)
            {
                // create custom exception with message that allows credit to be changed with debit.
                throw new InvalidOperationException("Wallet does not exist for credit.");
            }

            wallet.Balance += request.Amount;
            context.SaveChanges();
            logger.LogInformation(
                $"Wallet for business: {request.BusinessId}, credited with {wallet.Currency}{request.Amount}");
        }
    }
}