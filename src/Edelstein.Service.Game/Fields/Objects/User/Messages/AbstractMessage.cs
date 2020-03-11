using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages
{
    public abstract class AbstractMessage : IMessage
    {
        public abstract MessageType Type { get; }

        public void Encode(IPacket packet)
        {
            packet.Encode<byte>((byte) Type);
            EncodeData(packet);
        }

        protected abstract void EncodeData(IPacket packet);
    }
}