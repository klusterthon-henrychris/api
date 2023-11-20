using Kluster.Shared.Configuration;
using Kluster.Shared.Domain;
using Kluster.UserModule.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Kluster.UserModule.ModuleSetup
{
    public static class UserModuleServiceExtensions
    {
        internal static void AddCore(this IServiceCollection services)
        {
            AddMSIdentity(services);
            AddDatabase(services);
        }

        private static void RegisterDependencies(IServiceCollection services)
        { }

        private static void AddDatabase(IServiceCollection services)
        {
            var dbSettings = services.BuildServiceProvider().GetService<IOptions<DatabaseSettings>>()?.Value;
            services.AddDbContext<UserModuleDbContext>(options => options.UseSqlServer(dbSettings!.ConnectionString));
        }

        private static void AddMSIdentity(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<UserModuleDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
