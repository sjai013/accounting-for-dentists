using AccountingForDentists.Components.Pages.Contract.Shared;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Contract;

public partial class Add(IDbContextFactory<AccountingContext> contextFactory, TenantProvider tenantProvider, NavigationManager navigationManager)
{
    public string[] RegisteredBusinessNames { get; set; } = [];

    [SupplyParameterFromQuery]
    public string? ReturnUri { get; set; } = string.Empty;

    public async Task Submit(ContractViewModel Model)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        DateContainerEntity dateReference = new()
        {
            TenantId = tenantProvider.GetTenantId(),
            UserId = tenantProvider.GetUserObjectId(),
            DateContainerId = Guid.CreateVersion7(),
            Date = DateOnly.FromDateTime(Model.InvoiceDate)
        };
        SalesEntity salesEntity = new()
        {
            TenantId = tenantProvider.GetTenantId(),
            UserId = tenantProvider.GetUserObjectId(),
            SalesId = Guid.CreateVersion7(),
            Amount = Model.TotalSalesAmount,
            GST = Model.TotalSalesGSTAmount,
            DateReference = dateReference,
            BusinessName = Model.ClinicName,
            Description = "Services and Facilities Agreement Sales",
        };

        ExpensesEntity expensesEntity = new()
        {
            TenantId = tenantProvider.GetTenantId(),
            UserId = tenantProvider.GetUserObjectId(),
            ExpensesId = Guid.CreateVersion7(),
            Amount = Model.TotalExpensesAmount,
            GST = Model.TotalExpensesGSTAmount,
            DateReference = dateReference,
            BusinessName = Model.ClinicName,
            Description = "Services and Facilities Agreement Expenses",
        };

        ContractIncomeEntity contractIncomeEntity = new()
        {
            TenantId = tenantProvider.GetTenantId(),
            UserId = tenantProvider.GetUserObjectId(),
            ContractualAgreementId = Guid.CreateVersion7(),
            BusinessName = Model.ClinicName,
            InvoiceDateReference = dateReference,
            ExpensesEntity = expensesEntity,
            SalesEntity = salesEntity,
        };
        context.ContractIncome.Add(contractIncomeEntity);

        context.Sales.Add(salesEntity);
        context.Expenses.Add(expensesEntity);
        context.DateReferences.Add(dateReference);

        await context.SaveChangesAsync();
        NavigateBack();
    }

    public void NavigateBack()
    {
        if (!string.IsNullOrEmpty(ReturnUri))
        {
            navigationManager.NavigateTo(ReturnUri);
        }
    }
}
