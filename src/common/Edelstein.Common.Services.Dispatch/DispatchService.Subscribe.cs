using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using Edelstein.Protocol.Services.Dispatch.Contracts;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services.Dispatch;

public partial class DispatchService
{
    public IAsyncEnumerable<DispatchInfo> Subscribe(DispatchServiceSubscribeRequest request, CallContext context = default)
        => Subscribe(request, context.CancellationToken);

    private async IAsyncEnumerable<DispatchInfo> Subscribe(DispatchServiceSubscribeRequest request, [EnumeratorCancellation] CancellationToken token)
    {
        if (await _repository.Retrieve(request.ServerID) != null)
            yield break;
        
        var channel = Channel.CreateUnbounded<DispatchInfo>();
        var subscription = new DispatchSubscription
        {
            Channel = channel,
            ServerID = request.ServerID,
            WorldID = request.WorldID,
            ChannelID = request.ChannelID
        };
        
        await _repository.Insert(subscription);

        try
        {
            await foreach (var info in channel.Reader.ReadAllAsync(token))
                yield return info;
        }
        finally
        {
            Console.WriteLine("CLEANED");
            await _repository.Delete(subscription);
            channel.Writer.Complete();
        }
    }
}
