namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC
{
    public interface IFieldObjNPCInfo
    {
        public int ID { get; }

        public bool Move { get; }

        public int TrunkPut { get; }
        public int TrunkGet { get; }

        public bool IsTrunk => TrunkPut > 0 || TrunkGet > 0;
        public bool IsStoreBank { get; }
        public bool IsParcel { get; }

        public string Script { get; }
    }
}
