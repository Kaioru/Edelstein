using System.Threading.Channels;
using Edelstein.Protocol.Services.Dispatch.Contracts;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Services.Dispatch;

public record DispatchSubscription : DispatchServiceSubscribeRequest, IRepositoryEntry<string>
{
    public string ID => ServerID;
    public required ChannelWriter<DispatchInfo> Channel { get; init; }
}
