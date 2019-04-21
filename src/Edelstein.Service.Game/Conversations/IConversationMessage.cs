using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Conversations
{
    public interface IConversationMessage<in T>
    {
        ConversationMessageType Type { get; }

        void Encode(IPacket packet);
        bool Validate(T response);
    }
}