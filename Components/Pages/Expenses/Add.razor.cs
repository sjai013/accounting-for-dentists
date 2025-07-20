using System.Security.Cryptography;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Expenses;

public partial class Add(IDbContextFactory<AccountingContext> contextFactory, TenantProvider tenantProvider, NavigationManager navigationManager)
{
    [SupplyParameterFromQuery]
    public string? ReturnUri { get; set; } = string.Empty;

    private async Task Submit(Shared.ExpensesFormViewModel Model)
    {
        if (Model is null) return;
        var entityGuid = Guid.CreateVersion7();

        using var context = await contextFactory.CreateDbContextAsync();
        Guid tenantId = tenantProvider.GetTenantId();
        Guid userId = tenantProvider.GetUserObjectId();

        DateContainerEntity dateReference = new()
        {
            DateContainerId = Guid.CreateVersion7(),
            TenantId = tenantId,
            UserId = userId,
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
                TenantId = tenantProvider.GetTenantId(),
                UserId = tenantProvider.GetUserObjectId(),
                Bytes = Model.File.Bytes,
                SizeBytes = Model.File.Bytes.Length,
                Filename = Model.File.Filename,
                MD5Hash = md5hash
            };

            context.Attachments.Add(attachment);

        }

        ExpensesEntity entity = new()
        {
            ExpensesId = Guid.CreateVersion7(),
            TenantId = tenantId,
            UserId = userId,
            DateReference = dateReference,
            Amount = Model.Amount,
            GST = Model.GST,
            BusinessName = Model.BusinessName,
            Description = Model.Description,
            Attachment = attachment
        };
        context.Expenses.Add(entity);

        await context.SaveChangesAsync();
        NavigateBack();
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