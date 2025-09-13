using System.Security.Cryptography;
using AccountingForDentists.Components.Pages.Contract.Shared;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;

namespace AccountingForDentists.Components.Pages.Contract;

public static class HelperMethods
{
    public static void AddOrUpdate(AccountingContext context, ContractSubmitViewModel model, TenantProvider tenantProvider, ref ContractIncomeEntity entity)
    {
        AttachmentEntity? attachment = entity.Attachment;
        string md5Hash = string.Empty;
        if (model.File is not null)
        {
            md5Hash = Convert.ToHexStringLower(MD5.HashData(model.File.Bytes));
        }

        // An attachment is selected and it is new (no existing attachment)
        // Or it is different to the one that exists already
        if (
            model.File is not null &&
            (entity.Attachment is null
            || (entity.Attachment is not null && entity.Attachment.MD5Hash != md5Hash)))
        {
            FileEncryptionResult encryptionResult = AttachmentEntity.Encrypt(model.File.Bytes, tenantProvider.GetUserObjectId());

            entity.Attachment = new()
            {
                AttachmentId = Guid.CreateVersion7(),
                CustomerFilename = model.File.Filename,
                MD5Hash = md5Hash,
                SizeBytes = model.File.Bytes.Length,
                Key = encryptionResult.Key
            };
            context.Attachments.Add(entity.Attachment);
            var directory = tenantProvider.AttachmentsDirectory();
            var filePath = AttachmentEntity.GetPath(directory, entity.Attachment.AttachmentId);
            File.WriteAllBytes(filePath, encryptionResult.Bytes);
        }
        // There is an attachment already and it hasn't been removed.  Don't change the attachment
        else if (model.AttachmentId is not null)
        {
            // Do nothing, keep the existing attachment
        }
        // There is no attachemnt selected
        else if (model.File is null)
        {
            entity.Attachment = null;
        }

        entity.InvoiceDateReference.Date = model.InvoiceDate;

        if (entity.SalesEntity is null) // Add
        {
            entity.SalesEntity = new()
            {
                SalesId = Guid.CreateVersion7(),
                DateReference = entity.InvoiceDateReference
            };
            context.Sales.Add(entity.SalesEntity);
        }
        entity.SalesEntity.Amount = model.TotalSalesAmount;
        entity.SalesEntity.GST = model.TotalSalesGSTAmount;
        entity.SalesEntity.BusinessName = model.ClinicName;

        if (entity.ExpensesEntity is null)
        {
            entity.ExpensesEntity = new()
            {
                ExpensesId = Guid.CreateVersion7(),
                DateReference = entity.InvoiceDateReference
            };
            context.Add(entity.ExpensesEntity);
        }
        entity.ExpensesEntity.Amount = model.TotalExpensesAmount;
        entity.ExpensesEntity.GST = model.TotalExpensesGSTAmount;
        entity.ExpensesEntity.BusinessName = model.ClinicName;

        entity.BusinessName = model.ClinicName;
    }
}