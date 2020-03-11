using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Conversations
{
    public interface IConversationRequest<in T>
    {
        ConversationRequestType Type { get; }

        bool Validate(IConversationResponse<T> response);
        void Encode(IPacket packet);
    }
}