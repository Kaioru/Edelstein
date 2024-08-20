using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Services.Dispatch;
using Edelstein.Protocol.Services.Session;

namespace Edelstein.Common.Services.Dispatch;

public partial class DispatchService(
    ISessionService sessions
) : IDispatchService
{
    private readonly Repository<string, DispatchSubscription> _repository = new();
}
