using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;

namespace AccountingForDentists.Components.Pages.Business.Shared;

public partial class AddBusinessForm(AccountingContext context)
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
            context.Businesses.Add(businessEntity);
            await context.SaveChangesAsync();
            OnSaveSuccessful?.Invoke(businessEntity);
        }
        catch (Exception e)
        {

        }
        finally
        {
            Model = new();
        }
    }
}