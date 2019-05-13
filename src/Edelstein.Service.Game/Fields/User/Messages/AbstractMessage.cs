using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.User.Messages
{
    public abstract class AbstractMessage : IMessage
    {
        public abstract MessageType Type { get; }

        public abstract void Encode(IPacket packet);
    }
}