using Edelstein.Protocol.Services.Dispatch;
using Edelstein.Protocol.Services.Session;

namespace Edelstein.Common.Services.Dispatch;

public partial class DispatchService(
    ISessionService sessions
) : IDispatchService
{
    private readonly DispatchServiceEntryRepository<string> _serverIdIndex = new();
    private readonly DispatchServiceEntryRepository<int> _worldIdIndex = new();
    private readonly DispatchServiceEntryRepository<int> _channelIdIndex = new();
}
