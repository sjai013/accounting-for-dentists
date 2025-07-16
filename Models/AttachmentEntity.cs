namespace AccountingForDentists.Models;

public record AttachmentEntity
{
    public required Guid TenantId { get; set; }
    public required Guid UserId { get; set; }
    public required Guid AttachmentId { get; set; }
    public byte[] Bytes { get; set; } = [];
}
