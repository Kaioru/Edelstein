using System.Threading;
using System.Threading.Tasks;
using Edelstein.Network;
using Foundatio.AsyncEx;

namespace Edelstein.Service.Game.Conversation
{
    public interface IConversationContext
    {
        ISocket Socket { get; }
        CancellationTokenSource TokenSource { get; }
        ScriptMessageType ExpectedResponse { get; }
        AsyncProducerConsumerQueue<object> Responses { get; }

        Task<T> Send<T>(IMessage message);
    }
}