using System.Threading.Channels;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations.Messages;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations;

public class ConversationContext : IConversationContext
{
    private readonly IAdapter _adapter;
    private readonly Channel<object> _channel;
    public CancellationTokenSource TokenSource;

    public ConversationContext(IAdapter adapter)
    {
        _adapter = adapter;
        TokenSource = new CancellationTokenSource();
        _channel = Channel.CreateBounded<object>(new BoundedChannelOptions(1)
        {
            FullMode = BoundedChannelFullMode.DropWrite
        });
    }

    public async Task<T> Request<T>(IConversationMessageRequest<T> messageRequest)
    {
        await _adapter.Dispatch(new PacketWriter().Write(messageRequest));

        if (await _channel.Reader.ReadAsync(TokenSource.Token) is not IConversationMessageResponse<T> response)
            throw new InvalidDataException("Invalid response");
        if (messageRequest.Type != response.Type || !messageRequest.Check(response.Value))
            throw new InvalidDataException("Invalid response type or value");

        return response.Value;
    }

    public async Task<T> Respond<T>(IConversationMessageResponse<T> messageResponse)
    {
        await _channel.Writer.WriteAsync(messageResponse, TokenSource.Token);
        return messageResponse.Value;
    }

    public void Dispose()
    {
        TokenSource.Cancel();
        TokenSource.Dispose();

        _channel.Writer.Complete();
    }
}
