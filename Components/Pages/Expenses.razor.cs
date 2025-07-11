using System.Threading.Tasks;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages;

public partial class Expenses(AccountingContext context)
{
    public List<ExpensesEntity> ExpenseEntities { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        ExpenseEntities = await context.Expenses.OrderByDescending(x => x.Date).ToListAsync();
    }

    private async Task DeleteExpense(ExpensesEntity item)
    {
        context.Expenses.Remove(item);
        await context.SaveChangesAsync();
        ExpenseEntities.Remove(item);
        this.StateHasChanged();
    }
}