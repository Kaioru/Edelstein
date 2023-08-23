﻿using System.Threading.Channels;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Messages;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Game.Conversations;

public class ConversationContext : IConversationContext
{
    private readonly IAdapter _adapter;
    private readonly Channel<object> _channel;
    public CancellationTokenSource TokenSource { get; }

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
        await _adapter.Dispatch(new PacketWriter(PacketSendOperations.ScriptMessage)
            .Write(messageRequest)
            .Build());

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
