using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Field.User.Messages
{
    public interface IMessage
    {
        MessageType Type { get; }

        void Encode(IPacket packet);
    }
}