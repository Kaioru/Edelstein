using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

public class ConversationMessageResponse<T> : IConversationMessageResponse<T>
{

    public ConversationMessageResponse(ConversationMessageType type, T value)
    {
        Type = type;
        Value = value;
    }
    public ConversationMessageType Type { get; }
    public T Value { get; }
}
