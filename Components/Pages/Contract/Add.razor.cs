using System.Security.Cryptography;
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

    public string? Error { get; set; }

    public async Task Submit(ContractSubmitViewModel Model)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        try
        {

            using var transaction = await context.Database.BeginTransactionAsync();
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

            DateContainerEntity dateReference = new()
            {
                DateContainerId = Guid.CreateVersion7(),
                Date = DateOnly.FromDateTime(Model.InvoiceDate)
            };
            context.DateReferences.Add(dateReference);

            SalesEntity salesEntity = new()
            {
                SalesId = Guid.CreateVersion7(),
                Amount = Model.TotalSalesAmount,
                GST = Model.TotalSalesGSTAmount,
                DateReference = dateReference,
                BusinessName = Model.ClinicName,
                Description = "Services and Facilities Agreement Sales",
                Attachment = attachment
            };
            context.Sales.Add(salesEntity);

            ExpensesEntity expensesEntity = new()
            {
                ExpensesId = Guid.CreateVersion7(),
                Amount = Model.TotalExpensesAmount,
                GST = Model.TotalExpensesGSTAmount,
                DateReference = dateReference,
                BusinessName = Model.ClinicName,
                Description = "Services and Facilities Agreement Expenses",
                Attachment = attachment
            };
            context.Expenses.Add(expensesEntity);

            ContractIncomeEntity contractIncomeEntity = new()
            {
                ContractualAgreementId = Guid.CreateVersion7(),
                BusinessName = Model.ClinicName,
                InvoiceDateReference = dateReference,
                ExpensesEntity = expensesEntity,
                SalesEntity = salesEntity,
                Attachment = attachment
            };
            context.ContractIncome.Add(contractIncomeEntity);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
            NavigateBack();
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
    }

    public void NavigateBack()
    {
        if (!string.IsNullOrEmpty(ReturnUri))
        {
            navigationManager.NavigateTo(ReturnUri);
        }
    }
}
