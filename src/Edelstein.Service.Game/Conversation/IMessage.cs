using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Conversation
{
    public interface IMessage
    {
        ScriptMessageType Type { get; }
        ISpeaker Speaker { get; }

        void Encode(IPacket packet);
    }
}