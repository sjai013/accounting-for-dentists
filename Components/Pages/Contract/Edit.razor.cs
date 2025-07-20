using System.Security.Cryptography;
using AccountingForDentists.Components.Pages.Contract.Shared;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AccountingForDentists.Components.Pages.Contract;

public partial class Edit(IDbContextFactory<AccountingContext> contextFactory, TenantProvider tenantProvider, NavigationManager navigationManager)
{
    [Parameter]
    public required string EntityGuidString { get; set; }
    ContractViewModel InitialModel { get; set; } = new();

    [SupplyParameterFromQuery]
    public string? ReturnUri { get; set; } = string.Empty;

    public string? Error { get; set; }
    protected override async Task OnParametersSetAsync()
    {
        if (!Guid.TryParse(EntityGuidString, out var entityGuid))
        {
            return;
        }

        using var context = await contextFactory.CreateDbContextAsync();
        var model = await context.ContractIncome
        .Where(x => x.ContractualAgreementId == entityGuid)
                .Include(x => x.SalesEntity)
                .Include(x => x.ExpensesEntity)
                .Include(x => x.InvoiceDateReference)
                .Include(x => x.Attachment)
                .Select(x => new ContractViewModel()
                {
                    ClinicName = x.BusinessName,
                    InvoiceDate = x.InvoiceDateReference.Date.ToDateTime(new()),
                    TotalExpensesAmount = x.ExpensesEntity == null ? 0m : x.ExpensesEntity.Amount,
                    TotalExpensesGSTAmount = x.ExpensesEntity == null ? 0m : x.ExpensesEntity.GST,
                    TotalSalesAmount = x.SalesEntity == null ? 0m : x.SalesEntity.Amount,
                    TotalSalesGSTAmount = x.SalesEntity == null ? 0m : x.SalesEntity.GST,
                    AttachmentId = x.Attachment != null ? x.Attachment.AttachmentId : null,
                    File = x.Attachment != null ? new()
                    {
                        Filename = x.Attachment.CustomerFilename,
                        Size = x.Attachment.SizeBytes
                    } : null
                })
                .SingleOrDefaultAsync();

        if (model is null) return;
        InitialModel = model;
    }
    private async Task Submit(ContractSubmitViewModel args)
    {
        if (!Guid.TryParse(EntityGuidString, out var entityGuid))
        {
            return;
        }

        using var context = await contextFactory.CreateDbContextAsync();
        var entity = await context.ContractIncome.Where(x => x.ContractualAgreementId == entityGuid)
        .Include(x => x.SalesEntity)
        .Include(x => x.ExpensesEntity)
        .Include(x => x.InvoiceDateReference)
        .Include(x => x.Attachment)
        .SingleOrDefaultAsync();

        if (entity is null) return;
        try
        {

            using var transaction = await context.Database.BeginTransactionAsync();
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
                    CustomerFilename = args.File.Filename,
                    MD5Hash = md5hash,
                    SizeBytes = args.File.Bytes.Length,
                };
                entity.Attachment = attachment;
                context.Attachments.Add(attachment);

                var attachmentPath = attachment.GetPath(tenantProvider.AttachmentsDirectory());
                using var fs = new FileStream(attachmentPath, FileMode.Create, FileAccess.Write);
                fs.Write(args.File.Bytes);
            }

            entity.BusinessName = args.ClinicName;
            entity.InvoiceDateReference.Date = DateOnly.FromDateTime(args.InvoiceDate);
            if (entity.SalesEntity is not null)
            {
                entity.SalesEntity.Amount = args.TotalSalesAmount;
                entity.SalesEntity.GST = args.TotalSalesGSTAmount;
                entity.SalesEntity.BusinessName = args.ClinicName;
                context.Entry(entity.SalesEntity).State = EntityState.Modified;
            }

            if (entity.ExpensesEntity is not null)
            {
                entity.ExpensesEntity.Amount = args.TotalExpensesAmount;
                entity.ExpensesEntity.GST = args.TotalExpensesGSTAmount;
                entity.ExpensesEntity.BusinessName = args.ClinicName;
                context.Entry(entity.ExpensesEntity).State = EntityState.Modified;
            }

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