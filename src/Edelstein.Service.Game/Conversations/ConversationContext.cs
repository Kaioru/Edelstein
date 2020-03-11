using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Conversations
{
    public class ConversationContext : IConversationContext
    {
        private readonly Channel<object> _channel;

        public ISocket Socket { get; }
        public CancellationTokenSource TokenSource { get; }

        public ConversationContext(ISocket socket)
        {
            _channel = Channel.CreateBounded<object>(new BoundedChannelOptions(1)
            {
                FullMode = BoundedChannelFullMode.DropWrite
            });

            Socket = socket;
            TokenSource = new CancellationTokenSource();
        }

        public async Task<T> Request<T>(IConversationRequest<T> request)
        {
            using (var p = new Packet(SendPacketOperations.ScriptMessage))
            {
                request.Encode(p);
                await Socket.SendPacket(p);
            }

            var response = (IConversationResponse<T>) await _channel.Reader.ReadAsync(TokenSource.Token);

            if (request.Type != response.Type || !request.Validate(response))
                throw new InvalidDataException("Invalid response type or value");

            return response.Value;
        }

        public async Task Respond<T>(IConversationResponse<T> response)
            => await _channel.Writer.WriteAsync(response, TokenSource.Token);

        public void Dispose()
        {
            TokenSource?.Cancel();
                //TokenSource?.Dispose();
        }
    }
}