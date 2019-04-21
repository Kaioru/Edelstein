using System;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Network;

namespace Edelstein.Service.Game.Conversations
{
    public interface IConversationContext : IDisposable
    {
        ISocket Socket { get; }
        ConversationMessageType LastRequestType { get; }
        CancellationTokenSource TokenSource { get; }

        Task<T> Request<T>(IConversationMessage<T> message);
        Task Respond<T>(T response);
    }
}