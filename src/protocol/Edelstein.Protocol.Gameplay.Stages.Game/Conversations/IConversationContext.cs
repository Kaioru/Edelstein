using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Messages;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Conversations;

public interface IConversationContext : IDisposable
{
    Task<T> Request<T>(IConversationMessageRequest<T> messageRequest);
    Task<T> Respond<T>(IConversationMessageResponse<T> messageResponse);
}
