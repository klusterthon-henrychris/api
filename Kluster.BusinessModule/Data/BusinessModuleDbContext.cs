using Kluster.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kluster.BusinessModule.Data;

public class BusinessModuleDbContext(DbContextOptions<BusinessModuleDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers", t => t.ExcludeFromMigrations());
    }
    
    public DbSet<Business> Businesses { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
}