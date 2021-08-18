using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Messages;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Messages
{
    public abstract class AbstractMessage : IMessage
    {
        public abstract MessageType Type { get; }

        public void WriteToPacket(IPacketWriter writer)
        {
            WriteBase(writer);
            WriteData(writer);
        }

        public void WriteBase(IPacketWriter writer)
        {
            writer.WriteByte((byte)Type);
        }

        public abstract void WriteData(IPacketWriter writer);
    }
}
