using AccountingForDentists.Components.Pages.Contract.Shared;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AccountingForDentists.Components.Pages.Contract;

public partial class Edit(IDbContextFactory<AccountingContext> contextFactory, NavigationManager navigationManager)
{
    [Parameter]
    public required string EntityGuidString { get; set; }
    ContractViewModel InitialModel { get; set; } = new();

    [SupplyParameterFromQuery]
    public string? ReturnUri { get; set; } = string.Empty;
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
                    File = x.Attachment != null ? new()
                    {
                        Bytes = x.Attachment.Bytes,
                        Name = x.Attachment.Filename,
                        AttachmentId = x.Attachment.AttachmentId
                    } : null
                })
                .SingleOrDefaultAsync();

        if (model is null) return;
        InitialModel = model;
    }
    private async Task Submit(ContractViewModel args)
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

        if (args.File is null)
        {
            entity.Attachment = null;
        }
        else if (args.File.AttachmentId is null)
        {
            AttachmentEntity attachment = new()
            {
                AttachmentId = Guid.CreateVersion7(),
                Bytes = args.File.Bytes,
                Filename = args.File.Name,
                MD5Hash = args.File.MD5Hash,
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