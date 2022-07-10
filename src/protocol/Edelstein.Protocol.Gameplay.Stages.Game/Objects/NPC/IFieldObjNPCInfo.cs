using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC
{
    public interface IFieldObjNPCInfo : IRepositoryEntry<int>
    {
        public bool Move { get; }

        public int TrunkPut { get; }
        public int TrunkGet { get; }

        public bool IsTrunk => TrunkPut > 0 || TrunkGet > 0;
        public bool IsStoreBank { get; }
        public bool IsParcel { get; }

        public string Script { get; }
    }
}
