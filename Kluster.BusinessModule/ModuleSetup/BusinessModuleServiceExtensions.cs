using Kluster.BusinessModule.Data;
using Kluster.BusinessModule.Services;
using Kluster.BusinessModule.Services.Contracts;
using Kluster.Shared.Extensions;

namespace Kluster.BusinessModule.ModuleSetup
{
    public static class BusinessModuleServiceExtensions
    {
        internal static void AddCore(this IServiceCollection services)
        {
            DbExtensions.AddDatabase<BusinessModuleDbContext>(services);
            RegisterDependencies(services);
        }

        private static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddTransient<IBusinessService, BusinessService>();
        }
    }
}