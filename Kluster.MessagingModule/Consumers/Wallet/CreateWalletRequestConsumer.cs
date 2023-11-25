using Kluster.Shared.MessagingContracts.Events.Wallet;
using Kluster.Shared.SharedContracts.BusinessModule;
using MassTransit;

namespace Kluster.Messaging.Consumers.Wallet;

public class CreateWalletRequestConsumer(IWalletService walletService, ILogger<CreateWalletRequestConsumer> logger) : IConsumer<CreateWalletEvent>
{
    public Task Consume(ConsumeContext<CreateWalletEvent> context)
    {
        logger.LogInformation($"Received request to create wallet for business: {context.Message.BusinessId}.");
        return walletService.CreateWallet(context.Message);
    }
}