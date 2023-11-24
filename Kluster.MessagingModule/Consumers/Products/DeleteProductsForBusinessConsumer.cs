using Kluster.Shared.MessagingContracts.Commands.Products;
using Kluster.Shared.SharedContracts.BusinessModule;
using MassTransit;

namespace Kluster.Messaging.Consumers.Products;

public class DeleteProductsForBusinessConsumer(
    IProductService productService,
    ILogger<DeleteProductsForBusinessConsumer> logger) : IConsumer<DeleteProductsForBusiness>
{
    public Task Consume(ConsumeContext<DeleteProductsForBusiness> context)
    {
        logger.LogInformation($"Received request to delete products for business: {context.Message.BusinessId}.");
        return productService.DeleteAllProductsRelatedToBusiness(context.Message.BusinessId);
    }
}