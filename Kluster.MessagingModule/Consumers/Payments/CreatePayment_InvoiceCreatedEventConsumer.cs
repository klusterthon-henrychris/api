﻿using Kluster.Shared.MessagingContracts.Events.Invoices;
using Kluster.Shared.SharedContracts.PaymentModule;
using MassTransit;

namespace Kluster.Messaging.Consumers.Payments;

public class CreatePaymentInvoiceCreatedEventConsumer(IPaymentService paymentService, ILogger<CreatePaymentInvoiceCreatedEventConsumer> logger)
    : IConsumer<InvoiceCreatedEvent>
{
    public Task Consume(ConsumeContext<InvoiceCreatedEvent> context)
    {
        logger.LogInformation("Invoice {0} has been created. Creating Payment Object...", context.Message.InvoiceId);
        return paymentService.CreatePayment(context.Message);
    }
}