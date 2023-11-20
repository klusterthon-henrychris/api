namespace Kluster.BusinessModule.ModuleSetup
{
    public static class BusinessModule
    {
        public static void AddBusinessModule(this IServiceCollection services)
        {
            services.AddCore();
        }

        public static void UseBusinessModule(this WebApplication app)
        {
        }
    }
}