using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Session;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations
{
    public class ConversationContext : IConversationContext
    {
        private readonly ISession _session;

        private readonly CancellationTokenSource _tokenSource;
        private readonly Channel<object> _channel;

        public ConversationContext(ISession session)
        {
            _session = session;
            _tokenSource = new CancellationTokenSource();
            _channel = Channel.CreateBounded<object>(new BoundedChannelOptions(1)
            {
                FullMode = BoundedChannelFullMode.DropWrite
            });
        }

        public async Task<T> Request<T>(IConversationRequest<T> request)
        {
            await _session.Dispatch(
                new UnstructuredOutgoingPacket(PacketSendOperations.ScriptMessage)
                    .Write(request)
            );

            var response = (IConversationResponse<T>)await _channel.Reader.ReadAsync(_tokenSource.Token);

            if (request.Type != response.Type || !await request.Check(response.Value))
                throw new InvalidDataException("Invalid response type or value");

            return response.Value;
        }

        public async Task<T> Respond<T>(IConversationResponse<T> response)
        {
            await _channel.Writer.WriteAsync(response, _tokenSource.Token);
            return response.Value;
        }

        public void Dispose()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();

            _channel.Writer.Complete();
        }
    }
}
