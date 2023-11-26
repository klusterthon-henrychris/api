using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Kluster.PaymentModule.Services.Contracts;
using Kluster.Shared.Configuration;
using Kluster.Shared.DTOs.Requests.Payments;
using Kluster.Shared.SharedContracts.PaymentModule;
using Microsoft.Extensions.Options;
using RestSharp;

namespace Kluster.PaymentModule.Services;

public class PaystackService(
    IPayStackClient payStackClient,
    IOptionsSnapshot<PaystackSettings> options,
    ILogger<PaystackService> logger)
    : IPaystackService
{
    private readonly PaystackSettings _paystackSettings = options.Value;

    public bool IsRequestFromPaystack(string ipAddress)
    {
        return (_paystackSettings.AllowedIps ?? throw new InvalidOperationException("Paystack Settings not set!"))
            .Contains(ipAddress);
    }

    public async Task<bool> VerifyTransaction(string reference)
    {
        var paystackNotification = await payStackClient.VerifyTransaction(reference);
        return paystackNotification is null;
    }

    public bool IsRequestFromPaystack(string xPaystackHeader, PaystackNotification response)
    {
        var responseBody = JsonSerializer.Serialize(response);
        var result = "";
        var keyBytes = Encoding.UTF8.GetBytes(_paystackSettings.SecretKey);
        var inputBytes = Encoding.UTF8.GetBytes(responseBody);
        using (var hmac = new HMACSHA512(keyBytes))
        {
            var hashValue = hmac.ComputeHash(inputBytes);
            result = BitConverter.ToString(hashValue).Replace("-", string.Empty);
        }

        return result.ToLower().Equals(xPaystackHeader);
    }
}