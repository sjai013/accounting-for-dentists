using AccountingForDentists.Components.Pages.Contract.Shared;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;

namespace AccountingForDentists.Components.Pages.Contract;

public partial class Add(AccountingContext context, TenantProvider tenantProvider, NavigationManager navigationManager)
{
    public string[] RegisteredBusinessNames { get; set; } = [];

    public async Task Submit(ContractViewModel Model)
    {
        SalesEntity salesEntity = new()
        {
            TenantId = tenantProvider.GetTenantId(),
            UserId = tenantProvider.GetUserObjectId(),
            SalesId = Guid.CreateVersion7(),
            Amount = Model.TotalSalesAmount,
            GST = Model.TotalSalesGSTAmount,
            Date = DateOnly.FromDateTime(Model.InvoiceDate),
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
            Date = DateOnly.FromDateTime(Model.InvoiceDate),
            BusinessName = Model.ClinicName,
            Description = "Services and Facilities Agreement Expenses",
        };

        ContractualAgreementsEntity contractIncomeEntity = new()
        {
            TenantId = tenantProvider.GetTenantId(),
            UserId = tenantProvider.GetUserObjectId(),
            ContractualAgreementId = Guid.CreateVersion7(),
            BusinessName = Model.ClinicName,
            InvoiceDate = DateOnly.FromDateTime(Model.InvoiceDate),
            ExpensesEntity = expensesEntity,
            SalesEntity = salesEntity,
        };
        context.ContractIncome.Add(contractIncomeEntity);

        context.Sales.Add(salesEntity);
        context.Expenses.Add(expensesEntity);

        await context.SaveChangesAsync();
        var currentUri = navigationManager.Uri;
        navigationManager.NavigateTo(currentUri, true);
    }

}
