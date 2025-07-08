
using AccountingForDentists.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Infrastructure;

public class AccountingContext : DbContext
{
    public AccountingContext(DbContextOptions options) : base(options) { }

    public DbSet<SalesEntity> Sales { get; set; }
    public DbSet<ExpensesEntity> Expenses { get; set; }
    public DbSet<BusinessEntity> Businesses { get; set; }
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAutoscaleThroughput(400);

        modelBuilder.Entity<SalesEntity>()
        .ToContainer("Income")
        .HasNoDiscriminator()
        .HasPartitionKey(x => x.UserId)
        .HasKey(x => x.SalesId);
    }
}