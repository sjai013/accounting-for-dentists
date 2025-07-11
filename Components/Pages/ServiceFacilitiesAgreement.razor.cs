using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages;

public partial class ServiceFacilitiesAgreement(AccountingContext context, NavigationManager navigationManager)
{
    [SupplyParameterFromForm]
    public SFAViewModel Model { get; set; } = new();

    public string[] RegisteredBusinessNames { get; set; } = [];

    public async Task Submit()
    {
        SalesEntity salesEntity = new()
        {
            SalesId = Guid.CreateVersion7(),
            Amount = Model.TotalSalesAmount,
            GST = Model.TotalSalesGSTAmount,
            Date = DateOnly.FromDateTime(Model.InvoiceDate),
            BusinessName = Model.ClinicName,
            Description = "Services and Facilities Agreement Sales",
        };

        ExpensesEntity expensesEntity = new()
        {
            ExpensesId = Guid.CreateVersion7(),
            Amount = Model.TotalExpensesAmount,
            GST = Model.TotalExpensesGSTAmount,
            Date = DateOnly.FromDateTime(Model.InvoiceDate),
            BusinessName = Model.ClinicName,
            Description = "Services and Facilities Agreement Expenses",
            Sales = salesEntity
        };
        context.Sales.Add(salesEntity);
        context.Expenses.Add(expensesEntity);
        await context.SaveChangesAsync();
        var currentUri = navigationManager.Uri;
        navigationManager.NavigateTo(currentUri, true);
    }

    protected override async Task OnInitializedAsync()
    {
        RegisteredBusinessNames = await context.Businesses.Select(x => x.Name).ToArrayAsync();
    }

    public class SFAViewModel
    {
        public decimal TotalSalesAmount { get; set; }
        public decimal TotalSalesGSTAmount { get; set; }
        public decimal TotalExpensesAmount { get; set; }
        public decimal TotalExpensesGSTAmount { get; set; }
        public decimal IncomeAmount { get => (TotalSalesAmount + TotalSalesGSTAmount) - (TotalExpensesAmount + TotalExpensesGSTAmount); }
        public decimal IncomeGST { get => TotalExpensesGSTAmount - TotalSalesGSTAmount; }
        public decimal TotalIncome { get => IncomeAmount + IncomeGST; }
        public DateTime InvoiceDate { get; set; } = DateTime.Today;
        public string ClinicName { get; set; } = string.Empty;
    }


}
