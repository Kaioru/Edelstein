using System;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Conversations
{
    public interface IConversationContext : IDisposable
    {
        Task<T> Request<T>(IConversationRequest<T> request);
        Task<T> Respond<T>(IConversationResponse<T> response);
    }
}