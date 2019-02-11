using System;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Service.Game.Sockets;
using Foundatio.AsyncEx;

namespace Edelstein.Service.Game.Conversations
{
    public interface IConversationContext : IDisposable
    {
        WvsGameSocket Socket { get; }
        CancellationTokenSource TokenSource { get; }
        IScriptMessage PreviousScriptMessage { get; }
        AsyncProducerConsumerQueue<object> Responses { get; }

        Task<T> Send<T>(IScriptMessage scriptMessage);

        void Cancel();
    }
}