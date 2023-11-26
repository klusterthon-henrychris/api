using ErrorOr;
using Kluster.Shared.DTOs.Requests.Payments;
using Kluster.Shared.DTOs.Responses.Payments;
using Kluster.Shared.MessagingContracts.Commands.Payment;
using Kluster.Shared.MessagingContracts.Events;
using Kluster.Shared.MessagingContracts.Events.Invoices;

namespace Kluster.Shared.SharedContracts.PaymentModule;

public interface IPaymentService
{
    Task DeleteAllPaymentsLinkedToBusiness(DeletePaymentsForBusiness command);
    Task DeleteAllPaymentsLinkedToClient(DeletePaymentsForClient command);
    Task DeleteAllPaymentsLinkedToInvoice(DeletePaymentsForInvoice command);
    Task<ErrorOr<PaymentDetailsResponse>> GetPaymentDetails(string invoiceNo);

    Task CreatePayment(InvoiceCreatedEvent invoiceCreatedEvent);

    /// <summary>
    /// Verify origin of the request using ipAddress then queue payment completion 
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="ipAddress"></param>
    /// <returns></returns>
    Task<ErrorOr<Success>> ProcessPaymentNotification(PaystackNotification notification, string ipAddress);

    Task<ErrorOr<InvoicePaymentValidated>> IsPaystackTransactionValid(PaymentNotificationReceived contextMessage);
    Task<ErrorOr<Success>> CompletePayment(InvoicePaymentValidated invoiceCreatedEvent);
}