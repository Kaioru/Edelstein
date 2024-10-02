using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Messages;

namespace Edelstein.Common.Gameplay.Game.Dialogs.Conversations.Messages;

public record ConversationMessageAnswer<T>(
    ConversationMessageType Type, 
    T Value
) : IConversationMessageAnswer<T>;
