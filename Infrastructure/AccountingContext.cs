
using AccountingForDentists.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
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

        modelBuilder.Entity<SalesEntity>()
        .HasQueryFilter(o => o.TenantId == tenantId && o.UserObjectId == userObjectId)
        .HasNoDiscriminator()
        .HasPartitionKey(x => x.TenantId)
        .HasKey(x => x.SalesId);

        modelBuilder.Entity<ExpensesEntity>()
        .HasQueryFilter(o => o.TenantId == tenantId && o.UserObjectId == userObjectId)
        .HasNoDiscriminator()
        .HasPartitionKey(x => x.TenantId)
        .HasKey(x => x.ExpensesId);

        modelBuilder.Entity<BusinessEntity>()
        .HasQueryFilter(o => o.TenantId == tenantId && o.UserObjectId == userObjectId)
         .HasNoDiscriminator()
         .HasPartitionKey(x => x.TenantId)
         .HasKey(x => x.Name);

    }
}