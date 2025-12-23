namespace AccountingForDentists.API.Attachment;

public static class AttachmentEndpoints
{
    public static void MapAttachmentEndpoints(this WebApplication app)
    {
        app.MapGet("/api/attachment/{attachmentId}", async (string attachmentId, IAttachmentDownloader downloader) =>
        {
            if (!Guid.TryParse(attachmentId, out var parsedAttachmentId))
            {
                return Results.BadRequest("Invalid attachment ID format.");
            }

            var downloadResult = await downloader.Download(parsedAttachmentId);
            return Results.File(downloadResult.Bytes, downloadResult.ContentType, downloadResult.Filename);
        });
    }
}
