using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages;

public partial class Businesses(AccountingContext context)
{
    public List<BusinessEntity> BusinessEntities { get; set; } = [];
    protected override async Task OnInitializedAsync()
    {
        await UpdateBusinessEntities();
    }

    private async Task UpdateBusinessEntities()
    {
        BusinessEntities = await context.Businesses.ToListAsync();

    }
    private async void BusinessListUpdated(BusinessEntity args)
    {
        await UpdateBusinessEntities();
        this.StateHasChanged();
    }

    private async void DeleteBusinessItem(BusinessEntity entity)
    {
        context.Businesses.Remove(entity);
        await context.SaveChangesAsync();
        await UpdateBusinessEntities();
        this.StateHasChanged();
    }
}