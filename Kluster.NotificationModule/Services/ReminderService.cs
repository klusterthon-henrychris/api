using Hangfire;
using Kluster.NotificationModule.Services.Contracts;
using Kluster.Shared.Constants;
using Kluster.Shared.DTOs.Requests.Invoices;
using Kluster.Shared.MessagingContracts.Events.Invoices;
using Kluster.Shared.SharedContracts.BusinessModule;
using Kluster.Shared.SharedContracts.PaymentModule;
using MassTransit;

namespace Kluster.NotificationModule.Services;

public class ReminderService(
    IInvoiceService invoiceService,
    IBusinessService businessService,
    IBus bus,
    ILogger<ReminderService> logger)
    : IReminderService
{
    public async Task SendInvoiceReminder(InvoiceCreatedEvent invoiceCreatedEvent)
    {
        logger.LogInformation("Fetching invoice {0} to send reminder.", invoiceCreatedEvent.InvoiceId);
        var invoice = await invoiceService.GetInvoiceInternal(invoiceCreatedEvent.InvoiceId);
        if (invoice.Status == InvoiceStatus.Paid.ToString())
        {
            logger.LogInformation("Invoice {0} has been paid, ignoring reminder.", invoiceCreatedEvent.InvoiceId);

            RecurringJob.RemoveIfExists(invoice.InvoiceNo);
            return;
        }

        var businessName = await businessService.GetBusinessName(invoice.BusinessId) ?? "SimpleBiz";

        await bus.Publish(new SendInvoiceReminderRequest(invoice.InvoiceNo, invoiceCreatedEvent.FirstName,
            invoiceCreatedEvent.LastName, invoice.Amount, invoice.DueDate, invoice.DateOfIssuance,
            invoice.ClientEmailAddress, invoice.Status,
            businessName));
        logger.LogInformation("Reminder mail request published for invoice {0}.", invoiceCreatedEvent.InvoiceId);
    }
}