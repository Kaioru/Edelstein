namespace Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Messages;

public interface IConversationMessageResponse<out T> : IConversationMessage
{
    T Value { get; }
}
