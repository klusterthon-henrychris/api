using ErrorOr;
using Kluster.PaymentModule.Data;
using Kluster.PaymentModule.ServiceErrors;
using Kluster.Shared.Domain;
using Kluster.Shared.DTOs.Responses.Payments;
using Kluster.Shared.Exceptions;
using Kluster.Shared.MessagingContracts.Commands.Payment;
using Kluster.Shared.MessagingContracts.Events.Invoices;
using Kluster.Shared.ServiceErrors;
using Kluster.Shared.SharedContracts.PaymentModule;
using Microsoft.EntityFrameworkCore;

namespace Kluster.PaymentModule.Services;

public class PaymentService(PaymentModuleDbContext context, ILogger<PaymentService> logger) : IPaymentService
{
    public async Task DeleteAllPaymentsLinkedToBusiness(DeletePaymentsForBusiness command)
    {
        var payments = await context.Payments
            .Where(x => x.BusinessId == command.BusinessId)
            .ToListAsync();

        context.RemoveRange(payments);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAllPaymentsLinkedToClient(DeletePaymentsForClient command)
    {
        var payments = await context.Payments
            .Where(x => x.ClientId == command.ClientId)
            .ToListAsync();

        context.RemoveRange(payments);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAllPaymentsLinkedToInvoice(DeletePaymentsForInvoice command)
    {
        var payments = await context.Payments
            .Where(x => x.InvoiceId == command.InvoiceId)
            .ToListAsync();

        context.RemoveRange(payments);
        await context.SaveChangesAsync();
    }

    public async Task<ErrorOr<PaymentDetailsResponse>> GetPaymentDetails(string invoiceNo)
    {
        var invoice = await context.Invoices.FirstOrDefaultAsync(x => x.InvoiceNo == invoiceNo);
        if (invoice is null)
        {
            return SharedErrors<Invoice>.NotFound;
        }


        var payment = await context.Payments.FirstOrDefaultAsync(x => x.InvoiceId == invoiceNo);
        if (payment is null)
        {
            return SharedErrors<Payment>.NotFound;
        }

        // we only want incomplete payments
        if (!payment.IsCompleted)
        {
            return Errors.Invoice.PaymentAlreadyCompleted;
        }

        return new PaymentDetailsResponse(invoice.ClientEmailAddress, invoice.Amount * 100);
    }

    public async Task CreatePayment(InvoiceCreatedEvent invoiceCreatedEvent)
    {
        if (await context.Payments.AnyAsync(x => x.InvoiceId == invoiceCreatedEvent.InvoiceId))
        {
            logger.LogError($"Payment already exists for invoice: {invoiceCreatedEvent.InvoiceId}.");
            throw new PaymentAlreadyExistsException(
                $"Payment already exists for invoice: {invoiceCreatedEvent.InvoiceId}.");
        }

        var payment = PaymentModuleMapper.ToPayment(invoiceCreatedEvent);
        await context.AddAsync(payment);
        await context.SaveChangesAsync();
        logger.LogInformation($"Created payment for: {invoiceCreatedEvent.InvoiceId}.");
    }
}