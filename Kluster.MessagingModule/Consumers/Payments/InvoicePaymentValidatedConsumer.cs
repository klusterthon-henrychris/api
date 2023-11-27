using Kluster.Shared.MessagingContracts.Events;
using Kluster.Shared.SharedContracts.PaymentModule;
using MassTransit;

namespace Kluster.Messaging.Consumers.Payments;

public class InvoicePaymentValidatedConsumer(
    IPaymentService paymentService,
    IBus bus,
    ILogger<InvoicePaymentValidatedConsumer> logger) : IConsumer<InvoicePaymentValidated>
{
    public async Task Consume(ConsumeContext<InvoicePaymentValidated> context)
    {
        logger.LogInformation("Invoice {0} has been validated. Completing payment...", context.Message.InvoiceId);
        var result = await paymentService.CompletePayment(context.Message);
        if (result.IsError)
        {
            throw new InvalidOperationException(result.FirstError.Code);
        }

        // todo: send email and web notifications here.  describe flow
    }
}