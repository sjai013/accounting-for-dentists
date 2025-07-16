using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages;

public partial class Sales(AccountingContext context)
{
    private List<SalesEntity> SalesEntities { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        SalesEntities = await context.Sales.OrderByDescending(x => x.DateReference).ToListAsync();
    }

    private async Task DeleteSale(SalesEntity item)
    {
        context.Sales.Remove(item);
        await context.SaveChangesAsync();
        SalesEntities.Remove(item);
        this.StateHasChanged();
    }

}