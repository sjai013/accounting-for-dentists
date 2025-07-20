namespace AccountingForDentists.Models;

public record AttachmentEntity
{
    public required Guid AttachmentId { get; set; }
    public required string CustomerFilename { get; set; } = string.Empty;
    public required int SizeBytes { get; set; }
    public required string MD5Hash { get; set; } = string.Empty;

    public string GetPath(string directory)
    {
        return GetPath(directory, AttachmentId);
    }

    public static string GetPath(string directory, Guid Id)
    {
        return $"{directory}/{Id:N}";
    }
}
