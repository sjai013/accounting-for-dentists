using System.Security.Cryptography;
using AccountingForDentists.Components.Pages.Expenses.Shared;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Expenses;

public partial class Add(IDbContextFactory<AccountingContext> contextFactory, TenantProvider tenantProvider, NavigationManager navigationManager)
{
    [SupplyParameterFromQuery]
    public string? ReturnUri { get; set; } = string.Empty;

    public string? Error { get; set; }

    private async Task Submit(ExpensesFormSubmitViewModel Model)
    {
        if (Model is null) return;
        using var context = await contextFactory.CreateDbContextAsync();
        try
        {
            using var transaction = await context.Database.BeginTransactionAsync();

            DateContainerEntity dateReference = new()
            {
                DateContainerId = Guid.CreateVersion7(),
                Date = Model.InvoiceDate
            };
            context.DateReferences.Add(dateReference);

            AttachmentEntity? attachment = null;
            if (Model.File is not null && Model.File.Bytes.Length > 0)
            {
                string md5hash = Convert.ToHexStringLower(MD5.HashData(Model.File.Bytes));

                attachment = new()
                {
                    AttachmentId = Guid.CreateVersion7(),
                    SizeBytes = Model.File.Bytes.Length,
                    CustomerFilename = Model.File.Filename,
                    MD5Hash = md5hash
                };
                context.Attachments.Add(attachment);
                var attachmentPath = attachment.GetPath(tenantProvider.AttachmentsDirectory());
                using var fs = new FileStream(attachmentPath, FileMode.Create, FileAccess.Write);
                fs.Write(Model.File.Bytes);
            }

            ExpensesEntity entity = new()
            {
                ExpensesId = Guid.CreateVersion7(),
                DateReference = dateReference,
                Amount = Model.Amount,
                GST = Model.GST,
                BusinessName = Model.BusinessName,
                Description = Model.Description,
                Attachment = attachment
            };
            context.Expenses.Add(entity);
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