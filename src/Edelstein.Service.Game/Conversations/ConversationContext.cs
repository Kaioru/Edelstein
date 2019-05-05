using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Services;
using Foundatio.AsyncEx;

namespace Edelstein.Service.Game.Conversations
{
    public class ConversationContext : IConversationContext
    {
        public GameSocket Socket { get; }

        public ConversationMessageType LastRequestType { get; private set; }
        public CancellationTokenSource TokenSource { get; }

        private readonly AsyncProducerConsumerQueue<object> _responses;

        public ConversationContext(GameSocket socket)
        {
            Socket = socket;
            TokenSource = new CancellationTokenSource();
            _responses = new AsyncProducerConsumerQueue<object>();
        }

        public async Task<T> Request<T>(IConversationMessage<T> message)
        {
            LastRequestType = message.Type;

            using (var p = new Packet(SendPacketOperations.ScriptMessage))
            {
                message.Encode(p);
                await Socket.SendPacket(p);
            }

            var response = (T) await _responses.DequeueAsync(TokenSource.Token);

            if (!message.Validate(response))
                throw new InvalidDataException("Invalid response value");

            return response;
        }

        public async Task Respond<T>(T response)
        {
            await _responses.EnqueueAsync(response);
        }

        public void Dispose()
        {
            TokenSource?.Cancel();
            TokenSource?.Dispose();
            _responses?.CompleteAdding();
        }
    }
}