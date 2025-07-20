
using AccountingForDentists.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Infrastructure;

public class AccountingContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<SalesEntity> Sales { get; set; }
    public DbSet<ExpensesEntity> Expenses { get; set; }
    public DbSet<ContractIncomeEntity> ContractIncome { get; set; }
    public DbSet<DateContainerEntity> DateReferences { get; set; }
    public DbSet<BusinessEntity> Businesses { get; set; }
    public DbSet<AttachmentEntity> Attachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SetPrimaryKeys(modelBuilder);
    }

    private static void SetPrimaryKeys(ModelBuilder modelBuilder)
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

}