using Kluster.BusinessModule.Data;
using Kluster.Shared.Infrastructure;

namespace Kluster.BusinessModule.ModuleSetup
{
    public static class BusinessModuleServiceExtensions
    {
        internal static void AddCore(this IServiceCollection services)
        {
            DbExtensions.AddDatabase<BusinessModuleDbContext>(services);
        }
    }
}