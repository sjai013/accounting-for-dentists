
using AccountingForDentists.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Infrastructure;

public class AccountingContext(DbContextOptions options, TenantProvider tenantProvider) : DbContext(options)
{
    public DbSet<SalesEntity> Sales { get; set; }
    public DbSet<ExpensesEntity> Expenses { get; set; }
    public DbSet<ContractIncomeEntity> ContractIncome { get; set; }
    public DbSet<DateContainerEntity> DateReferences { get; set; }
    public DbSet<BusinessEntity> Businesses { get; set; }
    public DbSet<AttachmentEntity> Attachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SetQueryFilters(modelBuilder);
        SetPrimaryKeys(modelBuilder);
        SetIndex(modelBuilder);
    }

    private void SetQueryFilters(ModelBuilder modelBuilder)
    {
        var tenantId = tenantProvider.GetTenantId();
        var userObjectId = tenantProvider.GetUserObjectId();

        modelBuilder.Entity<SalesEntity>()
        .HasQueryFilter(x => x.TenantId == tenantId && x.UserId == userObjectId);

        modelBuilder.Entity<ExpensesEntity>()
        .HasQueryFilter(x => x.TenantId == tenantId && x.UserId == userObjectId);

        modelBuilder.Entity<BusinessEntity>()
        .HasQueryFilter(x => x.TenantId == tenantId && x.UserId == userObjectId);

        modelBuilder.Entity<ContractIncomeEntity>()
        .HasQueryFilter(x => x.TenantId == tenantId && x.UserId == userObjectId);

        modelBuilder.Entity<DateContainerEntity>()
        .HasQueryFilter(x => x.TenantId == tenantId && x.UserId == userObjectId);

        modelBuilder.Entity<AttachmentEntity>()
        .HasQueryFilter(x => x.TenantId == tenantId && x.UserId == userObjectId);
    }

    private void SetPrimaryKeys(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SalesEntity>()
        .HasKey(x => new { x.SalesId });

        modelBuilder.Entity<ExpensesEntity>()
        .HasKey(x => new { x.ExpensesId });

        modelBuilder.Entity<BusinessEntity>()
         .HasKey(x => new { x.Name });

        modelBuilder.Entity<ContractIncomeEntity>()
         .HasKey(x => new { x.ContractualAgreementId });

        modelBuilder.Entity<DateContainerEntity>()
            .HasKey(x => new { x.DateContainerId });

        modelBuilder.Entity<AttachmentEntity>()
            .HasKey(x => new { x.AttachmentId });
    }

    private void SetIndex(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SalesEntity>()
        .HasIndex(x => new { x.UserId, x.TenantId });

        modelBuilder.Entity<ExpensesEntity>()
        .HasIndex(x => new { x.TenantId, x.UserId });

        modelBuilder.Entity<BusinessEntity>()
        .HasIndex(x => new { x.TenantId, x.UserId });

        modelBuilder.Entity<ContractIncomeEntity>()
        .HasIndex(x => new { x.TenantId, x.UserId });

        modelBuilder.Entity<DateContainerEntity>()
        .HasIndex(x => new { x.TenantId, x.UserId });

        modelBuilder.Entity<AttachmentEntity>()
        .HasIndex(x => new { x.TenantId, x.UserId });
    }

}