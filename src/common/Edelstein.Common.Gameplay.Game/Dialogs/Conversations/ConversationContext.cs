using System;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Game.Dialogs.Conversations.Messages;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Messages;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Game.Dialogs.Conversations;

public class ConversationContext(
    ISocketUser user    
) : IConversationContext
{
    private readonly Channel<object> _channel = Channel.CreateBounded<object>(new BoundedChannelOptions(1)
    {
        FullMode = BoundedChannelFullMode.DropWrite
    });

    private readonly CancellationTokenSource _source = new();

    public CancellationToken Token => _source.Token;
    
    public async Task<T> Ask<T>(IConversationMessage<T> message)
    {
        await user.Dispatch(message.ToStructured());

        if (await _channel.Reader.ReadAsync(_source.Token) is not IConversationMessageAnswer<T> answer)
            throw new InvalidDataException("Invalid response");
        if (message.Type != answer.Type || !message.Check(answer.Value))
            throw new InvalidDataException("Invalid response type or value");

        return answer.Value;
    }
    
    public async Task<T> Answer<T>(IConversationMessageAnswer<T> message)
    {
        await _channel.Writer.WriteAsync(message, _source.Token);
        return message.Value;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        
        _source.Cancel();
        _source.Dispose();

        _channel.Writer.Complete();
    }
}
