using AccountingForDentists.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Contract.Shared;

public partial class ContractIncomeForm(AccountingContext context)
{
    [SupplyParameterFromForm]
    public ContractViewModel Model { get; set; } = new();

    [Parameter]
    public ContractViewModel? InitialModel { get; set; }

    string[] RegisteredBusinessNames { get; set; } = [];


    [Parameter]
    public required EventCallback<ContractViewModel> OnSubmit { get; set; }


    protected override async Task OnInitializedAsync()
    {
        RegisteredBusinessNames = await context.Businesses.Select(x => x.Name).ToArrayAsync();
    }

    protected override void OnParametersSet()
    {
        Model = InitialModel ?? Model;
    }

    private async Task Submit(Microsoft.AspNetCore.Components.Forms.EditContext args)
    {
        await OnSubmit.InvokeAsync(Model);

    }
}

public class ContractViewModel
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