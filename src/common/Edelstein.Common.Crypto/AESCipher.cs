using System.Buffers;
using System.Security.Cryptography;

namespace Edelstein.Common.Crypto;

public class AESCipher
{
    private readonly SymmetricAlgorithm _cipher;

    public AESCipher() : this(new byte[] { 0x13, 0x08, 0x06, 0xb4, 0x1b, 0x0f, 0x33, 0x52 })
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

    public void Transform(Span<byte> input, int remaining, uint pSrc)
    {
        var length = 0x5B0;
        var start = 0;

        const int srcExpL = sizeof(int) * 4;
        var srcExp = ArrayPool<byte>.Shared.Rent(srcExpL);
        var srcBytes = BitConverter.GetBytes(pSrc);

        using var crypt = _cipher.CreateEncryptor();

        while (remaining > 0)
        {
            for (var i = 0; i < srcExpL; ++i)
                srcExp[i] = srcBytes[i % 4];

            if (remaining < length)
                length = remaining;

            for (var i = start; i < start + length; ++i)
            {
                var sub = i - start;

                if (sub % srcExpL == 0)
                    crypt.TransformBlock(srcExp, 0, srcExpL, srcExp, 0);

                input[i] ^= srcExp[sub % srcExpL];
            }

            start += length;
            remaining -= length;
            length = 0x5B4;
        }
        
        ArrayPool<byte>.Shared.Return(srcExp);
    }
}
