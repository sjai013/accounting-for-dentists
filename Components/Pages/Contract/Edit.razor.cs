using AccountingForDentists.Components.Pages.Contract.Shared;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Contract;

public partial class Edit(IDbContextFactory<AccountingContext> contextFactory)
{
    [Parameter]
    public required string EntityGuidString { get; set; }

    ContractIncomeEntity? entity { get; set; }
    ContractViewModel InitialModel { get; set; } = new();

    protected override async Task OnParametersSetAsync()
    {
        if (!Guid.TryParse(EntityGuidString, out var entityGuid))
        {
            return;
        }

        using var context = await contextFactory.CreateDbContextAsync();
        entity = await context.ContractIncome.Where(x => x.ContractualAgreementId == entityGuid)
                .Include(x => x.SalesEntity)
                .Include(x => x.ExpensesEntity)
                .SingleOrDefaultAsync();

        if (entity is null) return;

        InitialModel = new()
        {
            ClinicName = entity.BusinessName,
            InvoiceDate = entity.InvoiceDateReference.Date.ToDateTime(new()),
            TotalExpensesAmount = entity.ExpensesEntity?.Amount ?? 0,
            TotalExpensesGSTAmount = entity.ExpensesEntity?.GST ?? 0,
            TotalSalesAmount = entity.SalesEntity?.Amount ?? 0,
            TotalSalesGSTAmount = entity.SalesEntity?.GST ?? 0,
        };


    }
    private async Task Submit(ContractViewModel args)
    {
        if (entity is null) return;
        using var context = await contextFactory.CreateDbContextAsync();
        entity.BusinessName = args.ClinicName;

        context.Entry(entity).State = EntityState.Modified;

        await context.SaveChangesAsync();
    }
}