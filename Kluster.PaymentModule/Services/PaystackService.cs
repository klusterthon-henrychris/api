using Kluster.PaymentModule.Services.Contracts;
using Kluster.Shared.Configuration;
using Microsoft.Extensions.Options;

namespace Kluster.PaymentModule.Services;

public class PaystackService(IOptionsSnapshot<PaystackSettings> options) : IPaystackService
{
    private readonly PaystackSettings _paystackSettings = options.Value;

    public bool IsRequestFromPaystack(string ipAddress)
    {
        return _paystackSettings.AllowedIps.Contains(ipAddress);
    }
    
    // public bool IsRequestFromPaystack(string xPaystackHeader, PaystackNotification response)
    // {
    //     var responseBody = JsonSerializer.Serialize(response);
    //     string result = "";
    //     byte[] secretkeyBytes = Encoding.UTF8.GetBytes(_paystackSettings.SecretKey);
    //     byte[] inputBytes = Encoding.UTF8.GetBytes(responseBody);
    //     using (var hmac = new HMACSHA512(secretkeyBytes))
    //     {
    //         byte[] hashValue = hmac.ComputeHash(inputBytes);
    //         result = BitConverter.ToString(hashValue).Replace("-", string.Empty);
    //     }
    //
    //     return result.ToLower().Equals(xPaystackHeader);
    // }
}