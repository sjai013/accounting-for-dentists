using System.Security.Cryptography;
using AccountingForDentists.Components.Pages.Expenses.Shared;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Expenses;

public partial class Edit(IDbContextFactory<AccountingContext> contextFactory, NavigationManager navigationManager)
{
    [Parameter]
    public required string EntityGuidString { get; set; }

    [SupplyParameterFromQuery]
    public string? ReturnUri { get; set; } = string.Empty;

    ExpensesFormViewModel? Initial;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if (!Guid.TryParse(EntityGuidString, out var entityGuid))
        {
            return;
        }

        using var context = await contextFactory.CreateDbContextAsync();
        Initial = await context.Expenses
           .Where(x => x.ExpensesId == entityGuid)
           .Include(x => x.DateReference)
           .Include(x => x.Attachment)
           .Select(x => new ExpensesFormViewModel()
           {
               InvoiceDate = x.DateReference.Date,
               Amount = x.Amount,
               GST = x.GST,
               Description = x.Description,
               BusinessName = x.BusinessName,
               AttachmentId = x.Attachment != null ? x.Attachment.AttachmentId : null,
               File = x.Attachment == null ? null : new()
               {
                   Bytes = x.Attachment.Bytes,
                   Filename = x.Attachment.Filename
               }
           })
           .SingleOrDefaultAsync();

    }

    private async Task Submit(ExpensesFormViewModel args)
    {
        if (args is null) return;
        if (!Guid.TryParse(EntityGuidString, out var entityGuid))
        {
            return;
        }

        using var context = await contextFactory.CreateDbContextAsync();
        var entity = await context.Expenses
           .Where(x => x.ExpensesId == entityGuid)
           .Include(x => x.DateReference)
           .Include(x => x.Attachment)
           .SingleOrDefaultAsync();

        if (entity is null) return;

        entity.BusinessName = args.BusinessName;
        entity.DateReference.Date = args.InvoiceDate;

        entity.Amount = args.Amount;
        entity.GST = args.GST;
        entity.BusinessName = args.BusinessName;
        context.Entry(entity).State = EntityState.Modified;

        if (args.File is null)
        {
            entity.Attachment = null;
        }
        else if (args.AttachmentId is null)
        {
            string md5hash = Convert.ToHexStringLower(MD5.HashData(args.File.Bytes));

            AttachmentEntity attachment = new()
            {
                AttachmentId = Guid.CreateVersion7(),
                Bytes = args.File.Bytes,
                Filename = args.File.Filename,
                MD5Hash = md5hash,
                SizeBytes = args.File.Bytes.Length,
                TenantId = entity.TenantId,
                UserId = entity.UserId
            };
            entity.Attachment = attachment;
            context.Attachments.Add(attachment);

        }


        await context.SaveChangesAsync();
        NavigateBack();

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