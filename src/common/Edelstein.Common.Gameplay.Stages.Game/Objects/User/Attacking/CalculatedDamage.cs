using System;
using System.Collections.Generic;
using Edelstein.Common.Util;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Attacking;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Attacking
{
    public class CalculatedDamage : ICalculatedDamage
    {
        private static readonly int RND_SIZE = 7;

        public int InitSeed1 { get; private set; }
        public int InitSeed2 { get; private set; }
        public int InitSeed3 { get; private set; }

        private CRand32 RndGenForCharacter { get; set; }
        private CRand32 RndGenForMob { get; set; }

        public CalculatedDamage()
        {
            var random = new Random();

            SetSeed(random.Next(), random.Next(), random.Next());
        }

        public CalculatedDamage(int s1, int s2, int s3)
        {
            SetSeed(s1, s2, s3);
        }

        public void SetSeed(int s1, int s2, int s3)
        {
            InitSeed1 = s1;
            InitSeed2 = s2;
            InitSeed3 = s3;

            s1 |= 0x100000;
            s2 |= 0x1000;
            s3 |= 0x10;

            RndGenForCharacter = new CRand32(s1, s2, s3);
            RndGenForMob = new CRand32(s1, s2, s3);
        }

        public IEnumerable<ICalculatedDamageInfo> CalculateCharacterPDamage(IAttackInfo info)
        {
            var random = new Rotational<long>(new long[RND_SIZE]);

            RndGenForCharacter.Next(random.Array);

            return null;
        }

        public IEnumerable<ICalculatedDamageInfo> CalculateCharacterMDamage(IAttackInfo info)
        {
            var random = new Rotational<long>(new long[RND_SIZE]);

            RndGenForCharacter.Next(random.Array);

            return null;
        }

        public int CalculateMobPDamage(IMobAttackInfo info)
        {
            return 0;
        }

        public int CalculateMobMDamage(IMobAttackInfo info)
        {
            return 0;
        }
    }
}
