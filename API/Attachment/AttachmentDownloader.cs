using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.API.Attachment;

public class AttachmentDownloader(IDbContextFactory<AccountingContext> contextFactory, TenantProvider tenantProvider) : IAttachmentDownloader
{
    public async Task<IAttachmentDownloader.AttachmentDownloaderResult> Download(Guid attachmentId)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        AttachmentEntity? attachment = await context.Attachments.Where(x => x.AttachmentId == attachmentId).FirstOrDefaultAsync();
        if (attachment is null) return new()
        {
            Bytes = [],
            ContentType = "application/octet-stream",
            Filename = attachmentId.ToString()
        };

        var directory = tenantProvider.AttachmentsDirectory();
        var filePath = AttachmentEntity.GetPath(directory, attachmentId);

        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        byte[] fileBytes = new byte[fs.Length];
        fs.ReadExactly(fileBytes);
        byte[] outputBytes = [];
        if (attachment.Key.Length > 0)
        {
            FileDecryptionResult decryptionResult = attachment.Decrypt(fileBytes, tenantProvider.GetUserObjectId());
            outputBytes = decryptionResult.Bytes;
        }
        else
        {
            outputBytes = fileBytes;
        }

        return new IAttachmentDownloader.AttachmentDownloaderResult
        {
            Bytes = outputBytes,
            ContentType = "application/octet-stream",
            Filename = attachment.CustomerFilename
        };
    }
}
