using Kluster.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kluster.PaymentModule.Data;

public class PaymentModuleDbContext(DbContextOptions<PaymentModuleDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers", t => t.ExcludeFromMigrations());
        modelBuilder.Entity<Business>().ToTable("Businesses", t => t.ExcludeFromMigrations());
    }
    
    public DbSet<Invoice> Invoices { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
}