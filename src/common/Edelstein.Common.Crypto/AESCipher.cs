using System.Security.Cryptography;

namespace Edelstein.Common.Crypto;

public class AESCipher
{
    private readonly SymmetricAlgorithm _cipher;

    public AESCipher() : this(new byte[] { 0xB3, 0x2C, 0x96, 0x65, 0x99, 0x32, 0xD0, 0x41 })
    {
    }

    public AESCipher(ReadOnlySpan<byte> userKey)
    {
        var expandedKey = new byte[userKey.Length * 4];

        for (var i = 0; i < userKey.Length; i++)
            expandedKey[i * 4] = userKey[i];

        _cipher = Aes.Create();
        _cipher.KeySize = 256;
        _cipher.Key = expandedKey;
        _cipher.Mode = CipherMode.ECB;
    }

    public void Transform(Span<byte> input, uint pSrc)
    {
        var remaining = input.Length;
        var length = 0x5B0;
        var start = 0;

        var srcExp = new byte[sizeof(int) * 4];
        var srcBytes = BitConverter.GetBytes(pSrc);

        using var crypt = _cipher.CreateEncryptor();

        while (remaining > 0)
        {
            for (var i = 0; i < srcExp.Length; ++i)
                srcExp[i] = srcBytes[i % 4];

            if (remaining < length)
                length = remaining;

            for (var i = start; i < start + length; ++i)
            {
                var sub = i - start;

                if (sub % srcExp.Length == 0)
                {
                    var result = crypt.TransformFinalBlock(srcExp, 0, srcExp.Length);
                    Array.Copy(result, srcExp, srcExp.Length);
                }

                input[i] ^= srcExp[sub % srcExp.Length];
            }

            start += length;
            remaining -= length;
            length = 0x5B4;
        }
    }
}
