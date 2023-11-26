using Kluster.Shared.MessagingContracts.Commands.Wallet;
using Kluster.Shared.SharedContracts.BusinessModule;
using MassTransit;

namespace Kluster.Messaging.Consumers.Wallet;

public class CreateWalletRequestConsumer(IWalletService walletService, ILogger<CreateWalletRequestConsumer> logger) : IConsumer<CreateWalletCommand>
{
    public Task Consume(ConsumeContext<CreateWalletCommand> context)
    {
        logger.LogInformation($"Received request to create wallet for business: {context.Message.BusinessId}.");
        return walletService.CreateWallet(context.Message);
    }
}