using AccountingForDentists.Components.Pages.Contract.Shared;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Contract;

public partial class Edit(IDbContextFactory<AccountingContext> contextFactory, NavigationManager navigationManager)
{
    [Parameter]
    public required string EntityGuidString { get; set; }
    ContractIncomeEntity? entity { get; set; }
    ContractViewModel InitialModel { get; set; } = new();

    [SupplyParameterFromQuery]
    public string? ReturnUri { get; set; } = string.Empty;
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
                .Include(x => x.InvoiceDateReference)
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
        entity.InvoiceDateReference.Date = DateOnly.FromDateTime(args.InvoiceDate);
        if (entity.SalesEntity is not null)
        {
            entity.SalesEntity.Amount = args.TotalSalesAmount;
            entity.SalesEntity.GST = args.TotalSalesGSTAmount;
            entity.SalesEntity.BusinessName = args.ClinicName;
            context.Entry(entity.SalesEntity).State = EntityState.Modified;
        }

        if (entity.ExpensesEntity is not null)
        {
            entity.ExpensesEntity.Amount = args.TotalExpensesAmount;
            entity.ExpensesEntity.GST = args.TotalExpensesGSTAmount;
            entity.ExpensesEntity.BusinessName = args.ClinicName;
            context.Entry(entity.ExpensesEntity).State = EntityState.Modified;
        }

        context.Entry(entity).State = EntityState.Modified;
        context.Entry(entity.InvoiceDateReference).State = EntityState.Modified;

        await context.SaveChangesAsync();
        NavigateBack();

    }

    private void NavigateBack()
    {
        if (!string.IsNullOrEmpty(ReturnUri))
        {
            navigationManager.NavigateTo(ReturnUri);
        }
    }
    private void Cancel()
    {
        NavigateBack();
    }
}