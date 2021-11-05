namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Attacking
{
    public interface ICalculatedDamage
    {
        uint InitSeed1 { get; }
        uint InitSeed2 { get; }
        uint InitSeed3 { get; }

        void SetSeed(uint s1, uint s2, uint s3);

        void SkipCalculationForCharacterDamage();
        ICalculatedDamageInfo[] CalculateCharacterPDamage(IAttackInfo info);
        ICalculatedDamageInfo[] CalculateCharacterMDamage(IAttackInfo info);

        void SkipCalculationForMobDamage();
        int CalculateMobPDamage(IMobAttackInfo info);
        int CalculateMobMDamage(IMobAttackInfo info);
    }
}
