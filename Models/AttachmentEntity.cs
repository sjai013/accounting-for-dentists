using System.Security.Cryptography;
using System.Text;

namespace AccountingForDentists.Models;

public record AttachmentEntity
{
    public required Guid AttachmentId { get; set; }
    public required string CustomerFilename { get; set; } = string.Empty;
    public required int SizeBytes { get; set; }
    public required string MD5Hash { get; set; } = string.Empty;
    public required byte[] Key { get; set; }

    public string GetPath(string directory)
    {
        return GetPath(directory, AttachmentId);
    }

    public static string GetPath(string directory, Guid Id)
    {
        return $"{directory}/{Id:N}";
    }

    internal static FileEncryptionResult Encrypt(byte[] fileBytes, Guid userObjectId)
    {
        using var aesAlg = Aes.Create();
        aesAlg.IV = userObjectId.ToByteArray();

        using ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
        byte[] encryptedBytes = FileCryptography.Encrypt(fileBytes, encryptor);
        return new FileEncryptionResult()
        {
            Bytes = [.. encryptedBytes],
            Key = aesAlg.Key
        };
    }

    internal FileDecryptionResult Decrypt(byte[] encryptedBytes, Guid userObjectId)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = Key;
        aesAlg.IV = userObjectId.ToByteArray();
        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
        byte[] decryptedBytes = FileCryptography.Encrypt(encryptedBytes, decryptor);
        return new FileDecryptionResult()
        {
            Bytes = [.. decryptedBytes]
        };
    }
}

public record FileEncryptionResult
{
    public byte[] Bytes { get; set; } = [];
    public byte[] Key { get; set; } = new byte[32];
}

public record FileDecryptionResult
{
    public byte[] Bytes { get; set; } = [];
}