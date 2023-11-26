using System.Net.Http.Headers;
using Kluster.PaymentModule.Data;
using Kluster.PaymentModule.Services;
using Kluster.PaymentModule.Services.Contracts;
using Kluster.Shared.Configuration;
using Kluster.Shared.Extensions;
using Kluster.Shared.SharedContracts.PaymentModule;
using Microsoft.Extensions.Options;
using Refit;

namespace Kluster.PaymentModule.ModuleSetup
{
    public static class PaymentModuleServiceExtensions
    {
        internal static void AddCore(this IServiceCollection services)
        {
            DbExtensions.AddDatabase<PaymentModuleDbContext>(services);
            RegisterDependencies(services);
        }

        private static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddTransient<IInvoiceService, InvoiceService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IPaystackService, PaystackService>();

            using var scope = services.BuildServiceProvider().CreateScope();
            var options = scope.ServiceProvider.GetService<IOptions<PaystackSettings>>();
            services.AddRefitClient<IPayStackClient>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(options!.Value.BaseUrl!);
                    c.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", options!.Value.SecretKey);
                });
        }
    }
}