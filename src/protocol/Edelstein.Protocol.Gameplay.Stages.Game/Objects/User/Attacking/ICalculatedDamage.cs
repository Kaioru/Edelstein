﻿using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Attacking
{
    public interface ICalculatedDamage
    {
        int InitSeed1 { get; }
        int InitSeed2 { get; }
        int InitSeed3 { get; }

        void SetSeed(int s1, int s2, int s3);

        IEnumerable<ICalculatedDamageInfo> CalculateCharacterPDamage(IAttackInfo info);
        IEnumerable<ICalculatedDamageInfo> CalculateCharacterMDamage(IAttackInfo info);

        int CalculateMobPDamage(IMobAttackInfo info);
        int CalculateMobMDamage(IMobAttackInfo info);
    }
}
