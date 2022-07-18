using Edelstein.Common.Services.Session.Contracts;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Services.Session.Contracts;

namespace Edelstein.Common.Services.Session;

public class SessionService : ISessionService
{
    private readonly ISession _session = new Session { AccountID = 1, State = SessionState.LoggedIn };

    public Task<ISessionStartResponse> Start(ISessionStartRequest request) =>
        Task.FromResult<ISessionStartResponse>(new SessionStartResponse(SessionStartResult.Success, _session));

    public Task<ISessionUpdateResponse> Update(ISessionUpdateRequest request) =>
        Task.FromResult<ISessionUpdateResponse>(new SessionUpdateResponse(SessionUpdateResult.Success, _session));

    public Task<ISessionEndResponse> End(ISessionEndRequest request) =>
        Task.FromResult<ISessionEndResponse>(new SessionEndResponse(SessionEndResult.Success));
}
