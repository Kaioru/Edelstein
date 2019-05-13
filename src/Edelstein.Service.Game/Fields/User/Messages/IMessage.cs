using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.User.Messages
{
    public interface IMessage
    {
        MessageType Type { get; }
        void Encode(IPacket packet);
    }
}