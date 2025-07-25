using System.Security.Cryptography;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;

namespace AccountingForDentists.Components.Pages.Sales;

public static class HelperMethods
{
    public static void AddOrUpdate(AccountingContext context, Shared.SalesFormSubmitViewModel model, TenantProvider tenantProvider, ref SalesEntity entity)
    {
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
        // There is no attachemnt selected
        else if (model.File is null)
        {
            entity.Attachment = null;
        }

        entity.BusinessName = model.BusinessName;
        entity.DateReference.Date = model.InvoiceDate;
        entity.Description = model.Description;
        entity.Amount = model.Amount;
        entity.GST = model.GST;
        entity.BusinessName = model.BusinessName;
    }
}