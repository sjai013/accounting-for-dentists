using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AccountingForDentists.Components.Pages.Shared.InputFile;

public partial class FileSelector
{
    const int maxFileSize = 5000000;

    [Parameter]
    public FileViewModel Value { get; set; } = null!;
    string FormId { get; set; } = string.Empty;
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> Attributes { get; set; } = [];

    [Parameter]
    public required bool IsFileUploaded { get; set; }

    private string FileInputKey { get; set; } = Guid.NewGuid().ToString();

    [Parameter]
    public EventCallback<FileViewModel> OnFileSelected { get; set; }

    [Parameter]
    public EventCallback OnFileRemoved { get; set; }

    [Parameter]
    public EventCallback<FileViewModel> OnFileDownloadRequest { get; set; }

    protected override Task OnInitializedAsync()
    {
        base.OnInitializedAsync();
        if (Attributes?.TryGetValue("id", out object? idObject) == true)
        {
            FormId = idObject?.ToString() ?? "";
        }

        return Task.CompletedTask;
    }

    private async Task FileSelected(InputFileChangeEventArgs args)
    {
        using var stream = args.File.OpenReadStream(maxFileSize);
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        FileViewModel model = new()
        {
            Bytes = memoryStream.ToArray(),
            Filename = args.File.Name
        };

        await OnFileSelected.InvokeAsync(model);
    }
    private async Task FileRemoved(MouseEventArgs args)
    {
        await OnFileRemoved.InvokeAsync();
    }
    private async Task FileDownload(MouseEventArgs args)
    {
        await OnFileDownloadRequest.InvokeAsync(Value);
    }

}

public class FileViewModel
{
    public required string Filename { get; set; }
    public required byte[] Bytes { get; set; }
}