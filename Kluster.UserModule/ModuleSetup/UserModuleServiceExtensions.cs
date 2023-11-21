using Kluster.Shared.Domain;
using Kluster.Shared.Infrastructure;
using Kluster.Shared.SharedContracts.UserModule;
using Kluster.UserModule.Data;
using Kluster.UserModule.Services;
using Kluster.UserModule.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Kluster.UserModule.ModuleSetup
{
    public static class UserModuleServiceExtensions
    {
        internal static void AddCore(this IServiceCollection services)
        {
            AddMsIdentity(services);
            RegisterDependencies(services);
            DbExtensions.AddDatabase<UserModuleDbContext>(services);
        }

        private static void RegisterDependencies(IServiceCollection services)
        {
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<ITokenService, TokenService>();

            services.AddScoped<ICurrentUser, CurrentUser>();
        }

        private static void AddMsIdentity(IServiceCollection services)
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
