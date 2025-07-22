using System.Security.Cryptography;
using AccountingForDentists.Components.Pages.Expenses.Shared;
using AccountingForDentists.Components.Pages.Sales.Shared;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Sales;

public partial class Edit(IDbContextFactory<AccountingContext> contextFactory, NavigationManager navigationManager)
{
    [Parameter]
    public required string EntityGuidString { get; set; }

    [SupplyParameterFromQuery]
    public string? ReturnUri { get; set; } = string.Empty;

    public string? Error { get; set; }

    SalesFormViewModel? Initial;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if (!Guid.TryParse(EntityGuidString, out var entityGuid))
        {
            return;
        }

        using var context = await contextFactory.CreateDbContextAsync();
        Initial = await context.Sales
           .Where(x => x.SalesId == entityGuid)
           .Include(x => x.DateReference)
           .Include(x => x.Attachment)
           .Select(x => new SalesFormViewModel()
           {
               InvoiceDate = x.DateReference.Date,
               Amount = x.Amount,
               GST = x.GST,
               Description = x.Description,
               BusinessName = x.BusinessName,
               AttachmentId = x.Attachment != null ? x.Attachment.AttachmentId : null,
               File = x.Attachment == null ? null : new()
               {
                   Filename = x.Attachment.CustomerFilename,
                   Size = x.Attachment.SizeBytes
               }
           })
           .SingleOrDefaultAsync();

    }

    private async Task Submit(SalesFormSubmitViewModel model)
    {
        Error = null;
        if (model is null) return;
        if (!Guid.TryParse(EntityGuidString, out var entityGuid))
        {
            return;
        }

        using var context = await contextFactory.CreateDbContextAsync();
        var entity = await context.Sales
           .Where(x => x.SalesId == entityGuid)
           .Include(x => x.DateReference)
           .Include(x => x.Attachment)
           .SingleOrDefaultAsync();

        if (entity is null) return;

        context.Sales.Update(entity);

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

    private void NavigateBack()
    {
        if (!string.IsNullOrEmpty(ReturnUri))
        {
            navigationManager.NavigateTo(ReturnUri);
        }
    }
    private void Cancel()
    {
        NavigateBack();
    }

}