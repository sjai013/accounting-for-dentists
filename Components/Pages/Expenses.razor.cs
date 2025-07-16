using System.Threading.Tasks;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages;

public partial class Expenses(IDbContextFactory<AccountingContext> contextFactory)
{
    public List<ExpensesEntity> ExpenseEntities { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        ExpenseEntities = await context.Expenses.Include(x => x.DateReference).OrderByDescending(x => x.DateReference.Date).ToListAsync();
    }

    private async Task DeleteExpense(ExpensesEntity item)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        context.Expenses.Remove(item);
        await context.SaveChangesAsync();
        ExpenseEntities.Remove(item);
        this.StateHasChanged();
    }
}