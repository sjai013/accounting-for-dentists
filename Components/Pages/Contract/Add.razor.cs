using AccountingForDentists.Components.Pages.Contract.Shared;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Contract;

public partial class Add(IDbContextFactory<AccountingContext> contextFactory, TenantProvider tenantProvider, NavigationManager navigationManager)
{
    public string[] RegisteredBusinessNames { get; set; } = [];

    [SupplyParameterFromQuery]
    public string? ReturnUri { get; set; } = string.Empty;

    public async Task Submit(ContractViewModel Model)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        AttachmentEntity? attachment = null;
        if (Model.File is not null && Model.File.Bytes.Length > 0)
        {
            attachment = new()
            {
                AttachmentId = Guid.CreateVersion7(),
                TenantId = tenantProvider.GetTenantId(),
                UserId = tenantProvider.GetUserObjectId(),
                Bytes = Model.File.Bytes,
                SizeBytes = Model.File.Bytes.Length,
                Filename = Model.File.Name,
                MD5Hash = Model.File.MD5Hash
            };
        }

        DateContainerEntity dateReference = new()
        {
            TenantId = tenantProvider.GetTenantId(),
            UserId = tenantProvider.GetUserObjectId(),
            DateContainerId = Guid.CreateVersion7(),
            Date = DateOnly.FromDateTime(Model.InvoiceDate)
        };
        SalesEntity salesEntity = new()
        {
            TenantId = tenantProvider.GetTenantId(),
            UserId = tenantProvider.GetUserObjectId(),
            SalesId = Guid.CreateVersion7(),
            Amount = Model.TotalSalesAmount,
            GST = Model.TotalSalesGSTAmount,
            DateReference = dateReference,
            BusinessName = Model.ClinicName,
            Description = "Services and Facilities Agreement Sales",
            Attachment = attachment
        };

        ExpensesEntity expensesEntity = new()
        {
            TenantId = tenantProvider.GetTenantId(),
            UserId = tenantProvider.GetUserObjectId(),
            ExpensesId = Guid.CreateVersion7(),
            Amount = Model.TotalExpensesAmount,
            GST = Model.TotalExpensesGSTAmount,
            DateReference = dateReference,
            BusinessName = Model.ClinicName,
            Description = "Services and Facilities Agreement Expenses",
            Attachment = attachment
        };

        ContractIncomeEntity contractIncomeEntity = new()
        {
            TenantId = tenantProvider.GetTenantId(),
            UserId = tenantProvider.GetUserObjectId(),
            ContractualAgreementId = Guid.CreateVersion7(),
            BusinessName = Model.ClinicName,
            InvoiceDateReference = dateReference,
            ExpensesEntity = expensesEntity,
            SalesEntity = salesEntity,
            Attachment = attachment
        };
        context.ContractIncome.Add(contractIncomeEntity);
        context.Sales.Add(salesEntity);
        context.Expenses.Add(expensesEntity);
        context.DateReferences.Add(dateReference);

        if (attachment is not null) context.Attachments.Add(attachment);

        await context.SaveChangesAsync();
        NavigateBack();
    }

    public void NavigateBack()
    {
        if (!string.IsNullOrEmpty(ReturnUri))
        {
            navigationManager.NavigateTo(ReturnUri);
        }
    }
}
