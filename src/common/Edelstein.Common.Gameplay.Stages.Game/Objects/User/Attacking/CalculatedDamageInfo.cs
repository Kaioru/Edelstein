using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Attacking;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Attacking
{
    public record CalculatedDamageInfo : ICalculatedDamageInfo
    {
        public int Damage { get; }
        public bool IsCritical { get; }

        public CalculatedDamageInfo(int damage, bool isCritical = false)
        {
            Damage = damage;
            IsCritical = isCritical;
        }
    }
}