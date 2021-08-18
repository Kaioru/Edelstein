using Edelstein.Protocol.Network.Utils;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Messages
{
    public interface IMessage : IPacketWritable
    {
        MessageType Type { get; }
    }
}
