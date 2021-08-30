using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob
{
    public interface IFieldObjMobInfo : IRepositoryEntry<int>
    {
        public MoveAbilityType MoveAbility { get; }

        public short Level { get; }
        public int EXP { get; }
        public int MaxHP { get; }
        public int MaxMP { get; }
    }
}
