using System;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Network;

namespace Edelstein.Service.Game.Conversations
{
    public interface IConversationContext : IDisposable
    {
        ISocket Socket { get; }
        CancellationTokenSource TokenSource { get; }

        Task<T> Request<T>(IConversationRequest<T> request);
        Task Respond<T>(IConversationResponse<T> response);
    }
}