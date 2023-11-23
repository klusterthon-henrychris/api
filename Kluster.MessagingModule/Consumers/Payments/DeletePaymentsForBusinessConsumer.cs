using Kluster.Shared.MessagingContracts.Commands.Payment;
using Kluster.Shared.SharedContracts.PaymentModule;
using MassTransit;

namespace Kluster.Messaging.Consumers.Payments;

public class DeletePaymentsForBusinessConsumer(
    IPaymentService paymentService,
    ILogger<DeletePaymentsForBusinessConsumer> logger) : IConsumer<DeletePaymentsForBusiness>
{
    public Task Consume(ConsumeContext<DeletePaymentsForBusiness> context)
    {
        logger.LogInformation($"Received request to delete payments for business: {context.Message.BusinessId}.");
        return paymentService.DeleteAllPaymentsLinkedToBusiness(context.Message);
    }
}