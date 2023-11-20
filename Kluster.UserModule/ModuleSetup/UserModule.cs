namespace Kluster.UserModule.ModuleSetup
{
    public static class UserModule
    {
        public static void AddUserModule(this IServiceCollection services)
        {
            services.AddCore();
        }

        public static void UseUserModule(this WebApplication app)
        {
            
        }
    }
}
