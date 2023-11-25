using ErrorOr;
using Kluster.Shared.DTOs.Responses.Payments;
using Kluster.Shared.MessagingContracts.Commands.Payment;

namespace Kluster.Shared.SharedContracts.PaymentModule;

public interface IPaymentService
{
    Task DeleteAllPaymentsLinkedToBusiness(DeletePaymentsForBusiness command);
    Task DeleteAllPaymentsLinkedToClient(DeletePaymentsForClient command);
    Task DeleteAllPaymentsLinkedToInvoice(DeletePaymentsForInvoice command);
    Task<ErrorOr<PaymentDetailsResponse>> GetPaymentDetails(string invoiceNo);
}