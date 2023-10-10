using System.Security.Cryptography;

namespace Edelstein.Common.Crypto;

public class AESCipher
{
    private readonly ICryptoTransform _transformer;

    public AESCipher() : this(new byte[] { 0x13, 0x08, 0x06, 0xb4, 0x1b, 0x0f, 0x33, 0x52 })
    {
    }

    public AESCipher(ReadOnlySpan<byte> userKey)
    {
        var expandedKey = new byte[userKey.Length * 4];
        var cipher = Aes.Create();

        for (var i = 0; i < userKey.Length; i++)
            expandedKey[i * 4] = userKey[i];
        
        cipher.KeySize = 256;
        cipher.Key = expandedKey;
        cipher.Mode = CipherMode.ECB;
        _transformer = cipher.CreateEncryptor();
    }

    public void Transform(Span<byte> input, uint pSrc)
    {
        var remaining = input.Length;
        var length = 0x5B0;
        var start = 0;

        var srcExp = new byte[sizeof(int) * 4];
        var srcBytes = BitConverter.GetBytes(pSrc);

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
                    var result = _transformer.TransformFinalBlock(srcExp, 0, srcExp.Length);
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
