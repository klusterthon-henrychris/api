using Kluster.Shared.MessagingContracts.Commands.Invoice;
using Kluster.Shared.SharedContracts.PaymentModule;
using MassTransit;

namespace Kluster.Messaging.Consumers.Invoices;

public class DeleteInvoicesForClientConsumer(
    IInvoiceService invoiceService,
    ILogger<DeleteInvoicesForClientConsumer> logger) : IConsumer<DeleteInvoicesForClient>
{
    public Task Consume(ConsumeContext<DeleteInvoicesForClient> context)
    {
        logger.LogInformation("Received request to delete invoices for client: {0}.", context.Message.ClientId);
        return invoiceService.DeleteAllInvoicesLinkedToClient(context.Message);
    }
}