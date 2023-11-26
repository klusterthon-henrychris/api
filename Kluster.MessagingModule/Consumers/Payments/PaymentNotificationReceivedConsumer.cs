using Kluster.Shared.MessagingContracts.Events;
using Kluster.Shared.SharedContracts.PaymentModule;
using MassTransit;

namespace Kluster.Messaging.Consumers.Payments;

public class PaymentNotificationReceivedConsumer(
    IPaymentService paymentService,
    IBus bus,
    ILogger<PaymentNotificationReceivedConsumer> logger)
    : IConsumer<PaymentNotificationReceived>
{
    // todo: configure retry for this specific consumer
    public async Task Consume(ConsumeContext<PaymentNotificationReceived> context)
    {
        logger.LogInformation(
            $"Received transaction webhook from Paystack for reference: {context.Message.DataReference}.");
        var isTransactionValid = await paymentService.IsPaystackTransactionValid(context.Message);
        if (isTransactionValid.IsError)
        {
            throw new InvalidOperationException(isTransactionValid.FirstError.Code);
        }

        await bus.Publish(isTransactionValid.Value);
    }
}