using Edelstein.Network.Packet;

namespace Edelstein.Service.Game.Conversations
{
    public interface IMessage
    {
        ScriptMessageType Type { get; }
        ISpeaker Speaker { get; }

        void Encode(IPacket packet);
        bool Validate(object response);
    }
}