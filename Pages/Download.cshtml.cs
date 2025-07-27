using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Pages;

public class DownloadModel(IDbContextFactory<AccountingContext> contextFactory, TenantProvider tenantProvider) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid FileId { get; set; }
    public async Task<FileResult> OnGetAsync()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        AttachmentEntity? attachment = await context.Attachments.Where(x => x.AttachmentId == FileId).FirstOrDefaultAsync();
        if (attachment is null) return File([], "application/octet-stream", FileId.ToString()); ;

        var directory = tenantProvider.AttachmentsDirectory();
        var filePath = AttachmentEntity.GetPath(directory, FileId);

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

        return File(outputBytes, "application/octet-stream", attachment.CustomerFilename);
    }
}