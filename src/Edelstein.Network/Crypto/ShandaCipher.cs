using System;

namespace Edelstein.Network.Crypto
{
    public class ShandaCipher
    {
        public static void EncryptTransform(Span<byte> input)
        {
            var size = input.Length;
            for (var i = 0; i < 3; i++)
            {
                byte a = 0;
                byte c;
                for (var j = size; j > 0; j--)
                {
                    c = input[size - j];
                    c = RollLeft(c, 3);
                    c = (byte) (c + j);
                    c ^= a;
                    a = c;
                    c = RollRight(a, j);
                    c ^= 0xFF;
                    c += 0x48;
                    input[size - j] = c;
                }

                a = 0;
                for (var j = input.Length; j > 0; j--)
                {
                    c = input[j - 1];
                    c = RollLeft(c, 4);
                    c = (byte) (c + j);
                    c ^= a;
                    a = c;
                    c ^= 0x13;
                    c = RollRight(c, 3);
                    input[j - 1] = c;
                }
            }
        }

        public static void DecryptTransform(Span<byte> input)
        {
            var size = input.Length;
            for (var i = 0; i < 3; i++)
            {
                byte a;
                byte b = 0;
                byte c;
                for (var j = size; j > 0; j--)
                {
                    c = input[j - 1];
                    c = RollLeft(c, 3);
                    c ^= 0x13;
                    a = c;
                    c ^= b;
                    c = (byte) (c - j);
                    c = RollRight(c, 4);
                    b = a;
                    input[j - 1] = c;
                }

                b = 0;
                for (var j = size; j > 0; j--)
                {
                    c = input[size - j];
                    c -= 0x48;
                    c ^= 0xFF;
                    c = RollLeft(c, j);
                    a = c;
                    c ^= b;
                    c = (byte) (c - j);
                    c = RollRight(c, 3);
                    b = a;
                    input[size - j] = c;
                }
            }
        }

        private static byte RollLeft(byte value, int shift)
        {
            var num = (uint) (value << (shift % 8));
            return (byte) ((num & 0xff) | (num >> 8));
        }

        private static byte RollRight(byte value, int shift)
        {
            var num = (uint) ((value << 8) >> (shift % 8));
            return (byte) ((num & 0xff) | (num >> 8));
        }
    }
}