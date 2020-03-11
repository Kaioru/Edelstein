namespace Edelstein.Service.Game.Conversations
{
    public interface IConversationResponse<out T>
    {
        ConversationRequestType Type { get; }
        T Value { get; }
    }
}