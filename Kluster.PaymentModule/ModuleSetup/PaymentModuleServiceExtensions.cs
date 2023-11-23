using Kluster.PaymentModule.Data;
using Kluster.PaymentModule.Services;
using Kluster.Shared.Extensions;
using Kluster.Shared.SharedContracts;
using Kluster.Shared.SharedContracts.PaymentModule;

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
        }
    }
}