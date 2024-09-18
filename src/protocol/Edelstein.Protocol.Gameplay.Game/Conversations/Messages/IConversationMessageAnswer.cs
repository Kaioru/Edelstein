namespace Edelstein.Protocol.Gameplay.Game.Conversations.Messages;

public interface IConversationMessageAnswer<out T>
{
    ConversationMessageType Type { get; }
    T Value { get; }
}
