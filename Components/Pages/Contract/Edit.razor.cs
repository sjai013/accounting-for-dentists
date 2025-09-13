using AccountingForDentists.Components.Pages.Contract.Shared;
using AccountingForDentists.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

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
                    InvoiceDate = x.InvoiceDateReference.Date,
                    ExpensesAmounts = x.ExpensesEntity == null ? new List<ContractViewModel.AmountWithGST>() : new List<ContractViewModel.AmountWithGST> { new() { Amount = x.ExpensesEntity.Amount, GST = x.ExpensesEntity.GST } },
                    SalesAmounts = x.SalesEntity == null ? new List<ContractViewModel.AmountWithGST>() : new List<ContractViewModel.AmountWithGST> { new() { Amount = x.SalesEntity.Amount, GST = x.SalesEntity.GST } },
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
    private async Task Submit(ContractSubmitViewModel model)
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
        context.ContractIncome.Update(entity);
        try
        {
            using var transaction = await context.Database.BeginTransactionAsync();

            HelperMethods.AddOrUpdate(context, model, tenantProvider, ref entity);

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