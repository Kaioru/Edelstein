using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob
{
    public interface IFieldObjMobInfo : IRepositoryEntry<int>
    {
        public MoveAbilityType MoveAbility { get; }

        public short Level { get; }

        public int MaxHP { get; }
        public int MaxMP { get; }

        public int PAD { get; }
        public int PDR { get; }
        public int MAD { get; }
        public int MDR { get; }
        public int ACC { get; }
        public int EVA { get; }

        public int EXP { get; }
    }
}
