using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Shared;

public partial class AddBusinessForm(IDbContextFactory<AccountingContext> contextFactory)
{
    [Parameter]
    public Action<BusinessEntity>? OnSaveSuccessful { get; set; } = null;

    public async Task Submit()
    {
        if (Model is null) return;

        var businessEntity = new BusinessEntity()
        {
            Name = Model.BusinessName,
        };

        try
        {
            using var context = await contextFactory.CreateDbContextAsync();

            context.Businesses.Add(businessEntity);
            await context.SaveChangesAsync();
            OnSaveSuccessful?.Invoke(businessEntity);
        }
        catch
        {

        }
        finally
        {
            Model = new() { BusinessName = string.Empty };
        }
    }
}