using System;
using System.Security.Cryptography;

namespace Edelstein.Protocol.Network.Ciphers
{
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

            _cipher = new AesManaged
            {
                KeySize = 256,
                Key = expandedKey,
                Mode = CipherMode.ECB
            };
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
}