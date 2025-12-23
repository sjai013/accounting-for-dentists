using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Expenses;

public partial class Edit : ComponentBase
{
    [Parameter]
    public required Guid EntityId { get; set; }

    List<string>? SellerNames { get; set; } = [];
    Models.ExpensesEntity? ExpenseEntity { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadSellers();
        await LoadExpenseData();
        await base.OnInitializedAsync();
    }

    private async Task LoadSellers()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        SellerNames = await context.Expenses.Select(x => x.BusinessName).Distinct().ToListAsync();
    }
    private async Task LoadExpenseData()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        ExpenseEntity = await context.Expenses
        .Include(x => x.DateReference)
        .Include(x => x.Attachment)
        .FirstOrDefaultAsync(e => e.ExpensesId == EntityId);

        if (ExpenseEntity is null)
        {
            // Handle the case where the entity is not found
            throw new InvalidOperationException("Expense entity not found.");
        }
    }

    private void HandleSubmit()
    {
        // This method runs on the server when the form is submitted
        Console.WriteLine($"Submit");

    }

}