using System.Collections.Generic;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Attacking
{
    public interface ICalculatedDamage
    {
        int InitSeed1 { get; }
        int InitSeed2 { get; }
        int InitSeed3 { get; }

        void SetSeed(int s1, int s2, int s3);

        IEnumerable<ICalculatedDamageInfo> CalculateCharacterPDamage();
        IEnumerable<ICalculatedDamageInfo> CalculateCharacterMDamage();

        int CalculateMobPDamage();
        int CalculateMobMDamage();
    }
}
