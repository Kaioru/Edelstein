using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations
{
    public class ConversationResponse<T> : IConversationResponse<T>
    {
        public ConversationRequestType Type { get; }
        public T Value { get; }

        public ConversationResponse(ConversationRequestType type, T value)
        {
            Type = type;
            Value = value;
        }
    }
}
