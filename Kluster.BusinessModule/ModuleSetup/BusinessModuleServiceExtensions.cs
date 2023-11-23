using Kluster.BusinessModule.Data;
using Kluster.BusinessModule.Services;
using Kluster.BusinessModule.Services.Contracts;
using Kluster.Shared.Extensions;
using Kluster.Shared.SharedContracts;
using Kluster.Shared.SharedContracts.BusinessModule;

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
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IProductService, ProductService>();
        }
    }
}