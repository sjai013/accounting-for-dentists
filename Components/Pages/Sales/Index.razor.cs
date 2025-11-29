using AccountingForDentists.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

public partial class Index(IDbContextFactory<AccountingContext> contextFactory) : ComponentBase
{
    protected override async Task OnInitializedAsync()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var sales = await context.Sales.ToListAsync();
    }


}
