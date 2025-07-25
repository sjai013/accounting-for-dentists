using AccountingForDentists.Components.Pages.Businesses.Shared;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Businesses;

public partial class Index(IDbContextFactory<AccountingContext> contextFactory)
{
    public List<BusinessEntity>? BusinessEntities { get; set; }
    BusinessEntity? PendingDeleteBusiness { get; set; }
    DeleteModal DeleteConfirmModal { get; set; } = null!;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var waitTask = Task.Delay(500);
            await UpdateBusinessEntities();
            await waitTask;
            this.StateHasChanged();
        }

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

    private async Task MarkDeleteBusinessItem(BusinessEntity? entity)
    {
        this.PendingDeleteBusiness = entity;
        await DeleteConfirmModal.ShowModal(entity);
    }

    private async Task DeleteBusinessItem(BusinessEntity entity)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        context.Businesses.Remove(entity);
        await context.SaveChangesAsync();
        await UpdateBusinessEntities();
    }
}