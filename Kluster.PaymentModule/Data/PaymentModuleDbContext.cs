using Kluster.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kluster.PaymentModule.Data;

public class PaymentModuleDbContext(DbContextOptions<PaymentModuleDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(nameof(PaymentModule));
    }
    
    public DbSet<Invoice> Invoices { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
}