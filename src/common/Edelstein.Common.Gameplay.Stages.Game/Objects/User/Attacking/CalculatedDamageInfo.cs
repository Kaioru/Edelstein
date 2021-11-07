using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Attacking;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Attacking
{
    public record CalculatedDamageInfo : ICalculatedDamageInfo
    {
        public int Damage { get; }
        public bool IsCritical { get; }

        public CalculatedDamageInfo(int damage, bool isCritical)
        {
            Damage = damage;
            IsCritical = isCritical;
        }

        public CalculatedDamageInfo(int damage)
        {
            Damage = damage;
        }
    }
}
