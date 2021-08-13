using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements
{
    public abstract class AbstractMoveFragment
    {
        public MoveFragmentAttribute Attribute { get; }

        protected AbstractMoveFragment(MoveFragmentAttribute attribute, IPacketReader reader)
        {
            Attribute = attribute;
            ReadData(reader);
        }

        public virtual void Apply(MovePath path) { }

        public abstract void ReadData(IPacketReader reader);
        public abstract void WriteData(IPacketWriter writer);
    }
}
