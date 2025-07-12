
using AccountingForDentists.Components.Pages;
using AccountingForDentists.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Infrastructure;

public class AccountingContext(DbContextOptions options, TenantProvider tenantProvider) : DbContext(options)
{
    public DbSet<SalesEntity> Sales { get; set; }
    public DbSet<ExpensesEntity> Expenses { get; set; }
    public DbSet<BusinessEntity> Businesses { get; set; }

    public DbSet<ServiceFacilitiesAgreementEntity> ServiceFacilitiesAgreements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var tenantId = tenantProvider.GetTenantId();
        var userObjectId = tenantProvider.GetUserObjectId();


        modelBuilder.Entity<SalesEntity>()
        .HasQueryFilter(x => x.TenantId == tenantId && x.UserId == userObjectId)
        .HasKey(x => new { x.TenantId, x.UserId, x.SalesId });

        modelBuilder.Entity<ExpensesEntity>()
        .HasQueryFilter(x => x.TenantId == tenantId && x.UserId == userObjectId)
        .HasKey(x => new { x.TenantId, x.UserId, x.ExpensesId });

        modelBuilder.Entity<BusinessEntity>()
        .HasQueryFilter(x => x.TenantId == tenantId && x.UserId == userObjectId)
         .HasKey(x => new { x.TenantId, x.UserId, x.Name });

        modelBuilder.Entity<ServiceFacilitiesAgreementEntity>()
        .HasQueryFilter(x => x.TenantId == tenantId && x.UserId == userObjectId)
         .HasKey(x => new { x.TenantId, x.UserId, x.ServiceFacilityAgreementId });

    }
}