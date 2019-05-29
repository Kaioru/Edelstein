using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Broadcasts
{
    public abstract class AbstractBroadcastMessage : IBroadcastMessage
    {
        public abstract BroadcastMessageType Type { get; }

        public void Encode(IPacket packet)
        {
            packet.Encode<byte>((byte) Type);
            packet.Encode<bool>(Type == BroadcastMessageType.Slide);
            EncodeData(packet);
        }

        protected abstract void EncodeData(IPacket packet);
    }
}