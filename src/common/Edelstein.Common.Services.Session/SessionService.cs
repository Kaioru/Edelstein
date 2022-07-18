using Edelstein.Common.Services.Session.Contracts;
using Edelstein.Common.Services.Session.Models;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Services.Session.Contracts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Session;

public class SessionService : ISessionService
{
    private readonly IDbContextFactory<SessionDbContext> _dbFactory;
    private readonly ISession _session = new Session { AccountID = 1, State = SessionState.LoggedIn };
    private readonly TimeSpan _sessionDuration = TimeSpan.FromMinutes(5);

    public SessionService(IDbContextFactory<SessionDbContext> dbFactory) => _dbFactory = dbFactory;

    public async Task<ISessionStartResponse> Start(ISessionStartRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var session = await db.Sessions
                .Where(s => s.ID == request.AccountID)
                .FirstOrDefaultAsync();

            if (session?.DateExpire > now) return new SessionStartResponse(SessionStartResult.FailedAlreadyActive);
            if (session == null)
            {
                session = new SessionModel
                {
                    ID = request.AccountID,
                    State = SessionState.LoggedIn,
                    DateUpdated = now,
                    DateExpire = now.Add(_sessionDuration)
                };
                db.Sessions.Add(session);
            }
            else
            {
                session.State = SessionState.LoggedIn;
                session.DateUpdated = now;
                session.DateExpire = now.Add(_sessionDuration);
                db.Sessions.Update(session);
            }

            await db.SaveChangesAsync();

            return new SessionStartResponse(SessionStartResult.Success, session.Adapt<Session>());
        }
        catch (Exception e)
        {
            return new SessionStartResponse(SessionStartResult.Unknown);
        }
    }

    public Task<ISessionUpdateResponse> Update(ISessionUpdateRequest request) =>
        Task.FromResult<ISessionUpdateResponse>(new SessionUpdateResponse(SessionUpdateResult.Success, _session));

    public async Task<ISessionEndResponse> End(ISessionEndRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var session = await db.Sessions
                .Where(s => s.ID == request.AccountID)
                .Where(s => s.DateExpire > now)
                .FirstOrDefaultAsync();

            if (session == null) return new SessionEndResponse(SessionEndResult.FailedNotActive);

            db.Sessions.Remove(session);
            await db.SaveChangesAsync();

            return new SessionEndResponse(SessionEndResult.Success);
        }
        catch (Exception)
        {
            return new SessionEndResponse(SessionEndResult.FailedUnknown);
        }
    }
}
