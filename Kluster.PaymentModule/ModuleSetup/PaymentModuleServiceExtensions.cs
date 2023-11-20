using Kluster.PaymentModule.Data;
using Kluster.Shared.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Kluster.PaymentModule.ModuleSetup
{
    public static class PaymentModuleServiceExtensions
    {
        internal static void AddCore(this IServiceCollection services)
        {
            AddDatabase(services);
        }

        private static void AddDatabase(IServiceCollection services)
        {
            var dbSettings = services.BuildServiceProvider().GetService<IOptions<DatabaseSettings>>()?.Value;
            services.AddDbContext<PaymentModuleDbContext>(
                options => options.UseSqlServer(dbSettings!.ConnectionString));
        }
    }
}
