using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages;

public partial class Sales(IDbContextFactory<AccountingContext> contextFactory)
{
    private List<SalesEntity> SalesEntities { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        using var context = await contextFactory.CreateDbContextAsync();

        SalesEntities = await context.Sales.Include(x => x.DateReference).OrderByDescending(x => x.DateReference.Date).ToListAsync();
    }

    private async Task DeleteSale(SalesEntity item)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        context.Sales.Remove(item);
        await context.SaveChangesAsync();
        SalesEntities.Remove(item);
        this.StateHasChanged();
    }

}