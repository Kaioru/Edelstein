using System.Collections;

namespace Edelstein.Common.Util
{
    public readonly struct Flags
    {
        private readonly BitArray _bits;

        public Flags(int size)
            => _bits = new BitArray(size);

        public Flags(BitArray bits)
            => _bits = bits;

        public bool SetFlag(int index, bool value = true)
            => _bits[index] = value;

        public bool HasFlag(int index)
            => _bits[index];

        public byte[] ToArray()
        {
            var numBytes = _bits.Count / 8;

            if (_bits.Count % 8 != 0) numBytes++;

            var bytes = new byte[numBytes];
            var byteIndex = 0;
            var bitIndex = 0;

            for (int i = 0; i < _bits.Count; i++)
            {
                if (_bits[i])
                    bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));

                bitIndex++;

                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    byteIndex++;
                }
            }

            return bytes;
        }

        public Flags And(Flags b) => new(_bits.And(b._bits));
        public Flags Or(Flags b) => new(_bits.Or(b._bits));
        public Flags Xor(Flags b) => new(_bits.Xor(b._bits));
        public Flags Not() => new(_bits.Not());

        public static Flags operator &(Flags a, Flags b) => a.And(b);
        public static Flags operator |(Flags a, Flags b) => a.Or(b);
        public static Flags operator ^(Flags a, Flags b) => a.Xor(b);
        public static Flags operator ~(Flags a) => a.Not();
    }
}
