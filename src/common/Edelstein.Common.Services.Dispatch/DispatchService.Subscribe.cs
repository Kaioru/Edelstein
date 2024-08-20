using System.Collections.Generic;
using System.Threading.Channels;
using Edelstein.Protocol.Services.Dispatch.Contracts;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services.Dispatch;

public partial class DispatchService
{
    public async IAsyncEnumerable<DispatchInfo> Subscribe(DispatchServiceSubscribeRequest request, CallContext context = default)
    {
        var channel = Channel.CreateUnbounded<DispatchInfo>();
        
        await _serverIdIndex.Insert(new DispatchServiceEntry<string>(request.ServerID, channel));
        if (request.WorldID.HasValue) await _worldIdIndex.Insert(new DispatchServiceEntry<int>(request.WorldID.Value, channel));
        if (request.ChannelID.HasValue) await _channelIdIndex.Insert(new DispatchServiceEntry<int>(request.ChannelID.Value, channel));

        while (!context.CancellationToken.IsCancellationRequested)
            yield return await channel.Reader.ReadAsync(context.CancellationToken);
        
        await _serverIdIndex.Delete(request.ServerID);
        if (request.WorldID.HasValue) await _worldIdIndex.Delete(request.WorldID.Value);
        if (request.ChannelID.HasValue) await _channelIdIndex.Delete(request.ChannelID.Value);
        
        channel.Writer.Complete();
    }
}
