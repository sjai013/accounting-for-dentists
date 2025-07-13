using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages;

public partial class Businesses(IDbContextFactory<AccountingContext> contextFactory)
{
    public List<BusinessEntity> BusinessEntities { get; set; } = [];
    protected override async Task OnInitializedAsync()
    {
        await UpdateBusinessEntities();
    }

    private async Task UpdateBusinessEntities()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        BusinessEntities = await context.Businesses.ToListAsync();

    }
    private async void BusinessListUpdated(BusinessEntity args)
    {
        await UpdateBusinessEntities();
        this.StateHasChanged();
    }

    private async void DeleteBusinessItem(BusinessEntity entity)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        context.Businesses.Remove(entity);
        await context.SaveChangesAsync();
        await UpdateBusinessEntities();
        this.StateHasChanged();
    }
}