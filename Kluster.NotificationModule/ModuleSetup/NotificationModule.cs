﻿using Kluster.NotificationModule.Services;
using Kluster.NotificationModule.Services.Contracts;
using Kluster.Shared.SharedContracts.NotificationModule;

namespace Kluster.NotificationModule.ModuleSetup
{
    public static class NotificationModule
    {
        public static void AddNotificationModule(this IServiceCollection services)
        {
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IHangfireService, HangfireService>();
            services.AddTransient<IReminderService, ReminderService>();
        }
    }
}