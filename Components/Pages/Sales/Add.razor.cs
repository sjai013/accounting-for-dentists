using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Sales;

public partial class Add(IDbContextFactory<AccountingContext> contextFactory, NavigationManager navigationManager)
{
    [SupplyParameterFromQuery]
    public string? ReturnUri { get; set; } = string.Empty;

    public string? Error { get; set; }

    private async Task Submit(Shared.SalesFormSubmitViewModel model)
    {
        if (model is null) return;
        using var context = await contextFactory.CreateDbContextAsync();

        DateContainerEntity dateReference = new()
        {
            DateContainerId = Guid.CreateVersion7(),
            Date = model.InvoiceDate
        };
        context.DateReferences.Add(dateReference);

        SalesEntity entity = new()
        {
            SalesId = Guid.CreateVersion7(),
            DateReference = dateReference,
        };
        context.Sales.Add(entity);

        try
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            HelperMethods.AddOrUpdate(context, model, ref entity);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
            NavigateBack();
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
    }

    private Task Cancel()
    {
        NavigateBack();
        return Task.CompletedTask;
    }

    private void NavigateBack()
    {
        if (!string.IsNullOrEmpty(ReturnUri))
        {
            navigationManager.NavigateTo(ReturnUri);
        }
    }
}
