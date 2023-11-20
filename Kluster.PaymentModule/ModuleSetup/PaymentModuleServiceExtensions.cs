using Kluster.PaymentModule.Data;
using Kluster.Shared.Infrastructure;

namespace Kluster.PaymentModule.ModuleSetup
{
    public static class PaymentModuleServiceExtensions
    {
        internal static void AddCore(this IServiceCollection services)
        {
            DbExtensions.AddDatabase<PaymentModuleDbContext>(services);
        }
    }
}
