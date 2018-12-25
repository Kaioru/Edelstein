using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Fields.User.Messages
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