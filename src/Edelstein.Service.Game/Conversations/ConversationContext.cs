using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Foundatio.AsyncEx;

namespace Edelstein.Service.Game.Conversations
{
    public class ConversationContext : IConversationContext
    {
        public ISocket Socket { get; }
        public ConversationMessageType LastRequestType { get; private set; }

        private readonly AsyncProducerConsumerQueue<object> _responses;
        private readonly CancellationTokenSource _tokenSource;

        public ConversationContext(ISocket socket)
        {
            Socket = socket;
            _responses = new AsyncProducerConsumerQueue<object>();
            _tokenSource = new CancellationTokenSource();
        }

        public async Task<T> Request<T>(IConversationMessage<T> message)
        {
            LastRequestType = message.Type;

            using (var p = new Packet(SendPacketOperations.ScriptMessage))
            {
                message.Encode(p);
                await Socket.SendPacket(p);
            }

            var response = (T) await _responses.DequeueAsync(_tokenSource.Token);

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
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
            _responses?.CompleteAdding();
        }
    }
}