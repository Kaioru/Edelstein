using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Templates
{
    public class MobTemplate : IFieldObjMobInfo
    {
        public int ID { get; }

        public MoveAbilityType MoveAbility { get; }

        public short Level { get; }
        public int EXP { get; }
        public int MaxHP { get; }
        public int MaxMP { get; }
    }
}
