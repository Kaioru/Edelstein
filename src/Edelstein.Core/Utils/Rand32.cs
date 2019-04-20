using System;

namespace Edelstein.Core.Utils
{
    public class Rand32
    {
        public uint Seed1 { get; private set; }
        public uint Seed2 { get; private set; }
        public uint Seed3 { get; private set; }
        private uint _prevSeed1;
        private uint _prevSeed2;
        private uint _prevSeed3;

        private readonly object _lock = new object();

        public Rand32(uint seed1, uint seed2, uint seed3)
        {
            Seed1 = seed1;
            Seed2 = seed2;
            Seed3 = seed3;
            _prevSeed1 = seed1;
            _prevSeed2 = seed2;
            _prevSeed3 = seed3;
        }

        public static Rand32 Create()
        {
            var rand = new Random();
            return new Rand32(
                (uint) rand.Next(),
                (uint) rand.Next(),
                (uint) rand.Next()
            );
        }

        public int Random()
        {
            lock (_lock)
            {
                _prevSeed1 = Seed1;
                _prevSeed2 = Seed2;
                _prevSeed3 = Seed3;

                var result1 = (Seed1 << 12) ^ (Seed1 >> 19) ^ ((Seed1 >> 6) ^ Seed1 << 12) & 0x1FFF;
                var result2 = 16 * Seed2 ^ (Seed2 >> 25) ^ ((16 * Seed2) ^ (Seed2 >> 23)) & 0x7F;
                var result3 = (Seed3 >> 11) ^ (Seed3 << 17) ^ ((Seed3 >> 8) ^ (Seed3 << 17)) & 0x1FFFFF;

                Seed1 = result1;
                Seed2 = result2;
                Seed3 = result3;
                return (int) (result1 ^ result2 ^ result3);
            }
        }

        public int PrevRandom()
        {
            lock (_lock)
            {
                return (int) (
                    16 * (((_prevSeed3 & 0xFFFFFFF0) << 13) ^
                          (_prevSeed2 ^ ((_prevSeed1 & 0xFFFFFFFE) << 8)) & 0xFFFFFFF8) ^
                    ((_prevSeed1 & 0x7FFC0 ^ ((_prevSeed3 & 0x1FFFFF00 ^
                                               ((_prevSeed3 ^
                                                 ((_prevSeed1 ^ (((_prevSeed2 >> 2) ^ _prevSeed2 & 0x3F800000) >> 4)) >>
                                                  8)) >>
                                                3)) >> 2)) >> 6));
            }
        }
    }
}