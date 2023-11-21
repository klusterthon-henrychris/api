using Kluster.Shared.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kluster.UserModule.Data
{
    public class UserModuleDbContext(DbContextOptions<UserModuleDbContext> options)
        : IdentityDbContext<ApplicationUser>(options);
}