namespace Edelstein.Protocol.Gameplay.Stages.Game.Conversations
{
    public interface IConversationResponse<T>
    {
        ConversationRequestType Type { get; }
        T Value { get; }
    }
}
