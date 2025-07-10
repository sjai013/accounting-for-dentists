
using AccountingForDentists.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Infrastructure;

public class AccountingContext(DbContextOptions options, TenantProvider tenantProvider) : DbContext(options)
{
    public DbSet<SalesEntity> Sales { get; set; }
    public DbSet<ExpensesEntity> Expenses { get; set; }
    public DbSet<BusinessEntity> Businesses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        var tenantId = tenantProvider.TenantId;
        var userObjectId = tenantProvider.UserObjectId;

        if (string.IsNullOrWhiteSpace(tenantId) || string.IsNullOrWhiteSpace(userObjectId))
        {
            throw new Exception("tenantId or userId is null.  Database access is not authorised.");
        }

        modelBuilder.HasDefaultContainer($"tid-{tenantId}_oid-{userObjectId}");

        modelBuilder.Entity<SalesEntity>()
        .HasPartitionKey(x => new { x.SalesId })
        .HasKey(x => new { x.SalesId });

        modelBuilder.Entity<ExpensesEntity>()
        .HasPartitionKey(x => new { x.ExpensesId })
        .HasKey(x => new { x.ExpensesId });

        modelBuilder.Entity<BusinessEntity>()
         .HasPartitionKey(x => new { x.Name })
         .HasKey(x => new { x.Name });

    }
}