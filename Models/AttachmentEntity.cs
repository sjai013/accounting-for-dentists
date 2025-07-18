namespace AccountingForDentists.Models;

public record AttachmentEntity
{
    public required Guid TenantId { get; set; }
    public required Guid UserId { get; set; }
    public required Guid AttachmentId { get; set; }
    public required string Filename { get; set; } = string.Empty;
    public required int SizeBytes { get; set; }
    public required string MD5Hash { get; set; } = string.Empty;
    public required byte[] Bytes { get; set; } = [];
}
