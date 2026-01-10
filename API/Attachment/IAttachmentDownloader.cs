namespace AccountingForDentists.API.Attachment;

public interface IAttachmentDownloader
{
    Task<AttachmentDownloaderResult> Download(Guid attachmentId);

    public class AttachmentDownloaderResult
    {
        public byte[] Bytes { get; set; } = [];
        public string ContentType { get; set; } = string.Empty;
        public string Filename { get; set; } = string.Empty;
    }
}