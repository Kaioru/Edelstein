using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects.User.Broadcasts
{
    public interface IBroadcastMessage
    {
        BroadcastMessageType Type { get; }
        void Encode(IPacket packet);
    }
}