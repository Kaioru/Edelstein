using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Utils;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements
{
    public abstract class AbstractMoveFragment : IPacketReadable, IPacketWritable
    {
        public MoveFragmentAttribute Attribute { get; }

        protected AbstractMoveFragment(MoveFragmentAttribute attribute, IPacketReader reader)
        {
            Attribute = attribute;
            ReadFromPacket(reader);
        }

        public virtual void Apply(MovePath path) { }

        public void ReadFromPacket(IPacketReader reader)
        {
            ReadData(reader);
        }

        public void WriteToPacket(IPacketWriter writer)
        {
            WriteBase(writer);
            WriteData(writer);
        }

        protected void WriteBase(IPacketWriter writer)
        {
            writer.WriteByte((byte)Attribute);
        }

        protected abstract void ReadData(IPacketReader reader);
        protected abstract void WriteData(IPacketWriter writer);
    }
}
