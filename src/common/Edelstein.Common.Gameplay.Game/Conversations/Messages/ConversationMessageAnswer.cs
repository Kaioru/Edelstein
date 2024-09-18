using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;

namespace Edelstein.Common.Gameplay.Game.Conversations.Messages;

public record ConversationMessageAnswer<T>(
    ConversationMessageType Type, 
    T Value
) : IConversationMessageAnswer<T>;
