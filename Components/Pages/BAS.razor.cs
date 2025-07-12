
using AccountingForDentists.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages;

public partial class BAS(AccountingContext context, NavigationManager navigationManager)
{

    [SupplyParameterFromQuery]
    int FY { get; set; }

    private Dictionary<MonthModel, BASViewModel> MonthlyBAS { get; set; } = [];


    private async Task UpdateModel()
    {
        if (FY == default)
        {
            FY = DateTime.Now.AddMonths(6).Year;
        }

        DateOnly startDate = DateOnly.FromDateTime(new DateTime(FY - 1, 7, 1));
        DateOnly endDate = DateOnly.FromDateTime(new DateTime(FY, 7, 1));

        var expenses = await context.Expenses.Where(x => x.Date >= startDate && x.Date < endDate).ToListAsync();
        var sales = await context.Sales.Where(x => x.Date >= startDate && x.Date < endDate).ToListAsync();


        var monthlyExpenses = expenses.ToLookup(e => new MonthModel { Year = e.Date.Year, Month = e.Date.Month }, e => e);
        var monthlySales = sales.ToLookup(s => new MonthModel { Year = s.Date.Year, Month = s.Date.Month }, s => s);

        var uniqueMonths = monthlyExpenses.Select(x => x.Key).Union(monthlySales.Select(x => x.Key)).ToList();

        MonthlyBAS.Clear();
        foreach (var month in uniqueMonths)
        {
            Models.ExpensesEntity[] expenseInMonth = monthlyExpenses[month].ToArray();
            Models.SalesEntity[] salesInMonth = monthlySales[month].ToArray();

            BASViewModel BAS = new()
            {
                Sales = (int)salesInMonth.Sum(x => x.Amount),
                SalesGST = (int)salesInMonth.Sum(x => x.GST),
                Expenses = (int)expenseInMonth.Sum(x => x.Amount),
                ExpenseGST = (int)expenseInMonth.Sum(x => x.GST)
            };

            MonthlyBAS.Add(month, BAS);
        }

    }

    protected override async Task OnParametersSetAsync()
    {
        await UpdateModel();
    }



    private void ChangeFY(int FY)
    {
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

    private record MonthModel
    {
        public int Month { get; set; }
        public int Year { get; set; }

    }

    public class BASViewModel
    {
        public int Sales { get; set; }
        public int GSTFreeSales => Sales - SalesGST;
        public int SalesGST { get; set; }
        public int Expenses { get; set; }
        public int ExpenseGST { get; set; }

    }
}