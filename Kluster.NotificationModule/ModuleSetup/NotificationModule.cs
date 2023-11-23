using Kluster.NotificationModule.Services;
using Kluster.Shared.SharedContracts.NotificationModule;

namespace Kluster.NotificationModule.ModuleSetup
{
    public static class NotificationModule
    {
        public static void AddNotificationModule(this IServiceCollection services)
        {
            services.AddTransient<INotificationService, NotificationService>();
        }
    }
}