using Kluster.Shared.MessagingContracts.Commands.Clients;
using Kluster.Shared.SharedContracts.BusinessModule;
using MassTransit;

namespace Kluster.Messaging.Consumers.Clients;

public class DeleteClientsForBusinessConsumer(
    IClientService clientService,
    ILogger<DeleteClientsForBusinessConsumer> logger) : IConsumer<DeleteClientsForBusiness>
{
    public Task Consume(ConsumeContext<DeleteClientsForBusiness> context)
    {
        logger.LogInformation("Received request to delete clients for business: {0}.", context.Message.BusinessId);
        return clientService.DeleteAllClientsRelatedToBusiness(context.Message.BusinessId);
    }
}