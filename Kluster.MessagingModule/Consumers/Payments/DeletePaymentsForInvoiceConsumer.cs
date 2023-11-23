using Kluster.Shared.MessagingContracts.Commands.Payment;
using Kluster.Shared.SharedContracts;
using Kluster.Shared.SharedContracts.PaymentModule;
using MassTransit;

namespace Kluster.Messaging.Consumers.Payments;

public class DeletePaymentsForInvoiceConsumer(
    IPaymentService paymentService,
    ILogger<DeletePaymentsForInvoiceConsumer> logger) : IConsumer<DeletePaymentsForInvoice>
{
    public Task Consume(ConsumeContext<DeletePaymentsForInvoice> context)
    {
        logger.LogInformation($"Received request to delete payments for invoice: {context.Message.InvoiceId}.");
        return paymentService.DeleteAllPaymentsLinkedToInvoice(context.Message);
    }
}