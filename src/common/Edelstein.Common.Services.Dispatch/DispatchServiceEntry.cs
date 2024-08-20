using System.Threading.Channels;
using Edelstein.Protocol.Services.Dispatch.Contracts;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Services.Dispatch;

public record DispatchServiceEntry<TKey>(
    TKey ID,
    ChannelWriter<DispatchInfo> Channel
) : IRepositoryEntry<TKey>;
