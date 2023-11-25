using System.Diagnostics;
using Kluster.Shared.Constants;
using Kluster.Shared.Domain;
using Kluster.UserModule.Data;
using Microsoft.AspNetCore.Identity;

namespace Kluster.UserModule.ModuleSetup
{
    public static class UserModuleAppExtensions
    {
        public static async Task SeedDatabase(this WebApplication app)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("UserModule: database seeding starting.");
            await SeedDatabaseInternal(app);

            stopwatch.Stop();
            var elapsedTime = stopwatch.Elapsed;
            Console.WriteLine($"UserModule: database seeding completed in {elapsedTime.TotalMilliseconds}ms.");
        }

        private static async Task SeedDatabaseInternal(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<UserModuleDbContext>();
            
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await SeedRoles(roleManager);
            await SeedUsers(userManager);

            await context.SaveChangesAsync();
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { UserRoles.Admin, UserRoles.User };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            Console.WriteLine("UserModule: role seeding complete.");
        }

        private static async Task SeedUsers(UserManager<ApplicationUser> userManager)
        {
            var adminUser = CreateUser("Admin", "admin@example.com", UserRoles.Admin,
                "c0bdebd1-f275-4722-aa54-ca4524e4b998");
            var normalUser = CreateUser("User", "user@example.com", UserRoles.User,
                "337fa2c2-09bc-44e4-ac44-3d5641619829");

            await AddUser(userManager, adminUser, "secretPassword12@");
            await AddUser(userManager, normalUser, "secretPassword12@");

            Console.WriteLine("UserModule: User seeding complete.");
        }

        private static ApplicationUser CreateUser(string userName, string email, string role, string userId)
        {
            return new ApplicationUser
            {
                Id = userId,
                FirstName = userName,
                LastName = "Ihenacho",
                Email = email,
                NormalizedEmail = email.ToUpper(),
                UserName = userName,
                NormalizedUserName = userName.ToUpper(),
                Address = "",
                PhoneNumber = $"+1{userName.Length}1".PadLeft(12, '1'),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                Role = role
            };
        }

        private static async Task AddUser(UserManager<ApplicationUser> userManager, ApplicationUser user,
            string password)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (await userManager.FindByEmailAsync(user.Email!) is null)
            {
                var passwordHasher = new PasswordHasher<ApplicationUser>();
                var hashedPassword = passwordHasher.HashPassword(user, password);
                user.PasswordHash = hashedPassword;

                var result = await userManager.CreateAsync(user);
                await userManager.AddToRoleAsync(user, user.Role);
            }
        }
    }
}