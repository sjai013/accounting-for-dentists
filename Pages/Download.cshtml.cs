using AccountingForDentists.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Pages;

public class DownloadModel(IDbContextFactory<AccountingContext> contextFactory) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid FileId { get; set; }
    public async Task<FileResult> OnGetAsync()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        Models.AttachmentEntity? attachment = await context.Attachments.Where(x => x.AttachmentId == FileId).FirstOrDefaultAsync();
        if (attachment is null) return File([], "application/octet-stream", FileId.ToString()); ;
        return File(attachment.Bytes, "application/octet-stream", attachment.Filename);
    }
}