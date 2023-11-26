using Kluster.Shared.DTOs.Requests.Payments;
using Refit;

namespace Kluster.Shared.SharedContracts.PaymentModule;

public interface IPayStackClient
{
    [Get("/transaction/verify/{reference}")]
    Task<PaystackNotification?> VerifyTransaction(string reference);
}