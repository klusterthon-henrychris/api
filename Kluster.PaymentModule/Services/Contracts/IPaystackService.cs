using Kluster.Shared.DTOs.Requests.Payments;

namespace Kluster.PaymentModule.Services.Contracts;

public interface IPaystackService
{
    /// <summary>
    /// Verifies if the request comes from a whitelisted IP.
    /// </summary>
    /// <param name="ipAddress"></param>
    /// <returns></returns>
    bool IsRequestFromPaystack(string ipAddress);
}