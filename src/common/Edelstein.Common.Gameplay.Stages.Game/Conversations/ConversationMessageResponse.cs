using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Messages;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations;

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
