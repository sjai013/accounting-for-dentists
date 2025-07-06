
using AccountingForDentists.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Infrastructure;

public class AccountingContext : DbContext
{
    public AccountingContext(DbContextOptions options) : base(options) { }

    public DbSet<Income> Income { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAutoscaleThroughput(400);

        modelBuilder.Entity<Income>()
        .HasNoDiscriminator()
        .HasPartitionKey(x => x.TenantId)
        .HasKey(x => x.IncomeId);
    }
}