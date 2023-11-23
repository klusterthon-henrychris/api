using Kluster.Shared.MessagingContracts.Commands.Invoice;
using Kluster.Shared.SharedContracts.PaymentModule;
using MassTransit;

namespace Kluster.Messaging.Consumers.Invoices;

public class DeleteInvoicesForBusinessConsumer(
    IInvoiceService invoiceService,
    ILogger<DeleteInvoicesForBusinessConsumer> logger) : IConsumer<DeleteInvoicesForBusiness>
{
    public Task Consume(ConsumeContext<DeleteInvoicesForBusiness> context)
    {
        logger.LogInformation($"Received request to delete invoices for business: {context.Message.BusinessId}.");
        return invoiceService.DeleteAllInvoicesLinkedToBusiness(context.Message);
    }
}