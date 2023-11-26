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

    /// <summary>
    /// Checks if a Paystack transaction notification is valid by comparing it to the existing invoice.
    /// </summary>
    /// <param name="contextMessage">The payment notification received.</param>
    /// <returns>An asynchronous task that represents the operation and contains the result of the validation.</returns>
    Task<ErrorOr<InvoicePaymentValidated>> IsPaystackTransactionNotificationValid(PaymentNotificationReceived contextMessage);

    /// <summary>
    /// Completes a payment process. It validates the payment and invoice details, credits the wallet, 
    /// updates the payment and invoice status, and saves the changes in the database.
    /// </summary>
    /// <param name="invoiceCreatedEvent">The invoice payment validated event.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains either an Error or Success.</returns>
    Task<ErrorOr<Success>> CompletePayment(InvoicePaymentValidated invoiceCreatedEvent);
}