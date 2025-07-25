using System.Security.Cryptography;

namespace AccountingForDentists;

public static class FileCryptography
{
    public static byte[] Encrypt(byte[] data, ICryptoTransform encryptor)
    {
        return PerformCryptography(data, encryptor);
    }

    public static byte[] Decrypt(byte[] data, ICryptoTransform decryptor)
    {
        return PerformCryptography(data, decryptor);
    }

    public static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
    {
        using var inputMemoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(inputMemoryStream, cryptoTransform, CryptoStreamMode.Write);
        cryptoStream.Write(data, 0, data.Length);
        cryptoStream.FlushFinalBlock();
        return inputMemoryStream.ToArray();
    }

}