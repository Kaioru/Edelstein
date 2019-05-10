using System;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Service.Game.Services;

namespace Edelstein.Service.Game.Conversations
{
    public interface IConversationContext : IDisposable
    {
        GameSocket Socket { get; }
        ConversationMessageType LastRequestType { get; }
        CancellationTokenSource TokenSource { get; }

        Task<T> Request<T>(IConversationMessage<T> message);
        Task Respond<T>(T response);
    }
}