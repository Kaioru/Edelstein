using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;

namespace Edelstein.Protocol.Gameplay.Game.Conversations;

public interface IConversationContext : IDisposable
{
    Task<T> Request<T>(IConversationMessageRequest<T> messageRequest);
    Task<T> Respond<T>(IConversationMessageResponse<T> messageResponse);
}
