using Kluster.BusinessModule.Data;
using Kluster.Shared.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Kluster.BusinessModule.ModuleSetup
{
    public static class BusinessModuleServiceExtensions
    {
        internal static void AddCore(this IServiceCollection services)
        {
            AddDatabase(services);
        }

        private static void AddDatabase(IServiceCollection services)
        {
            var dbSettings = services.BuildServiceProvider().GetService<IOptions<DatabaseSettings>>()?.Value;
            services.AddDbContext<BusinessModuleDbContext>(
                options => options.UseSqlServer(dbSettings!.ConnectionString));
        }
    }
}