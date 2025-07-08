using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages;

public partial class Counter(AccountingContext context)
{
    private int currentCount = 0;

    private async void IncrementCount()
    {
        context.Income.Add(new SalesEntity
        {
            SalesId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Amount = 100,
            Date = DateOnly.FromDateTime(DateTime.Now.ToLocalTime()),
            Description = "Test Income"
        });
        await context.SaveChangesAsync();
        var items = await context.Income.ToListAsync();
        currentCount = items.Count;
    }
}