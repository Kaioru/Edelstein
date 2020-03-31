namespace Edelstein.Service.Game.Conversations
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