using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Messages
{
    public abstract class AbstractMessage : IMessage
    {
        public abstract MessageType Type { get; }

        public void Encode(IPacketEncoder packet)
        {
            packet.EncodeByte((byte) Type);
            EncodeData(packet);
        }

        protected abstract void EncodeData(IPacketEncoder packet);
    }
}