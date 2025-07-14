using AccountingForDentists.Components.Pages.Shared;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Contract;

public partial class Index(AccountingContext context, NavigationManager navigationManager)
{
    List<ContractIncomeEntity> SFAEntities = [];

    [SupplyParameterFromQuery]
    public string? Business { get; set; }

    [SupplyParameterFromQuery]
    public int FY { get; set; }

    List<OptionList<string>.Option> Businesses { get; set; } = [];

    protected override void OnInitialized()
    {
        FY = DateTime.Now.AddMonths(6).Year;
    }
    protected override async Task OnParametersSetAsync()
    {
        var sfaEntitiesQuery = context.ContractIncome
            .Include(x => x.SalesEntity)
            .Include(x => x.ExpensesEntity)
            .Where(x => x.BusinessName == Business || Business == null);

        if (FY != default)
        {
            DateOnly startDate = DateOnly.FromDateTime(new DateTime(FY - 1, 7, 1));
            DateOnly endDate = DateOnly.FromDateTime(new DateTime(FY, 7, 1));
            sfaEntitiesQuery = sfaEntitiesQuery.Where(x => x.InvoiceDate >= startDate && x.InvoiceDate < endDate);
        }

        this.SFAEntities = await sfaEntitiesQuery
            .OrderByDescending(x => x.InvoiceDate)
            .ToListAsync();

        var businessesEntities = await context.Businesses.OrderBy(x => x.Name).ToListAsync();

        this.Businesses = [new OptionList<string>.Option() { Label = "", Value = null }, .. businessesEntities.Select(x => new OptionList<string>.Option() { Label = x.Name, Value = x.Name })];
    }

    private void SelectBusiness(string? selectedBusiness)
    {
        string newUri;
        Dictionary<string, object?> uriParams = new()
        {
            ["Business"] = selectedBusiness,
            ["FY"] = FY
        };

        newUri = navigationManager.GetUriWithQueryParameters(uriParams);
        Business = selectedBusiness;
        navigationManager.NavigateTo(newUri);
    }

    void SelectFY(int FY)
    {
        this.FY = FY;
        string newUri;
        if (FY == default)
        {
            newUri = navigationManager.GetUriWithQueryParameter("FY", (int?)null);
        }
        else
        {
            newUri = navigationManager.GetUriWithQueryParameter("FY", FY);
        }
        navigationManager.NavigateTo(newUri);
    }

    async Task DeleteSFA(ContractIncomeEntity item)
    {
        // Delete expense
        if (item.ExpensesEntity is not null) context.Expenses.Remove(item.ExpensesEntity);

        // Delete sale
        if (item.SalesEntity is not null) context.Sales.Remove(item.SalesEntity);

        // Delete SFA record
        context.ContractIncome.Remove(item);

        SFAEntities.Remove(item);

        await context.SaveChangesAsync();
    }
}