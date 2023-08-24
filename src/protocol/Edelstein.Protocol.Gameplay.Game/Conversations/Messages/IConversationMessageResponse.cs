namespace Edelstein.Protocol.Gameplay.Game.Conversations.Messages;

public interface IConversationMessageResponse<out T> : IConversationMessage
{
    T Value { get; }
}
