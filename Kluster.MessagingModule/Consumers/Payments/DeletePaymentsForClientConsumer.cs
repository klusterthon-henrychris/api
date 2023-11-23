using Kluster.Shared.MessagingContracts.Commands.Payment;
using Kluster.Shared.SharedContracts.PaymentModule;
using MassTransit;

namespace Kluster.Messaging.Consumers.Payments;

public class DeletePaymentsForClientConsumer(
    IPaymentService paymentService,
    ILogger<DeletePaymentsForClientConsumer> logger) : IConsumer<DeletePaymentsForClient>
{
    public Task Consume(ConsumeContext<DeletePaymentsForClient> context)
    {
        logger.LogInformation($"Received request to delete payments for client: {context.Message.ClientId}.");
        return paymentService.DeleteAllPaymentsLinkedToClient(context.Message);
    }
}