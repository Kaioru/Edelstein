using Edelstein.Common.Services.Server.Entities;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Services.Session.Contracts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Server;

public class SessionService : ISessionService
{
    private readonly IDbContextFactory<ServerDbContext> _dbFactory;

    public SessionService(IDbContextFactory<ServerDbContext> dbFactory) => _dbFactory = dbFactory;

    public async Task<SessionResponse> Start(SessionStartRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Sessions.FindAsync(request.Session.ActiveAccount);

            if (existing != null)
            {
                var migration = await db.Migrations
                    .FirstOrDefaultAsync(m => m.AccountID == request.Session.ActiveAccount);

                if (migration == null || migration.DateExpire > now)
                    return new SessionResponse(SessionResult.FailedAlreadyStarted);

                db.Sessions.Remove(existing);
                db.Migrations.Remove(migration);
            }

            db.Sessions.Add(request.Session.Adapt<SessionEntity>());
            await db.SaveChangesAsync();
            return new SessionResponse(SessionResult.Success);
        }
        catch (Exception)
        {
            return new SessionResponse(SessionResult.FailedUnknown);
        }
    }

    public async Task<SessionResponse> End(SessionEndRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var session = await db.Sessions.FindAsync(request.ID);

            if (session == null)
                return new SessionResponse(SessionResult.FailedNotStarted);

            db.Sessions.Remove(session);
            await db.SaveChangesAsync();
            return new SessionResponse(SessionResult.Success);
        }
        catch (Exception)
        {
            return new SessionResponse(SessionResult.FailedUnknown);
        }
    }

    public async Task<SessionResponse> UpdateServer(SessionUpdateServerRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var session = await db.Sessions.FindAsync(request.ID);

            if (session == null)
                return new SessionResponse(SessionResult.FailedNotStarted);

            session.ServerID = request.ServerID;
            db.Sessions.Update(session);
            await db.SaveChangesAsync();
            return new SessionResponse(SessionResult.Success);
        }
        catch (Exception)
        {
            return new SessionResponse(SessionResult.FailedUnknown);
        }
    }

    public async Task<SessionResponse> UpdateCharacter(SessionUpdateCharacterRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var session = await db.Sessions.FindAsync(request.ID);

            if (session == null)
                return new SessionResponse(SessionResult.FailedNotStarted);

            session.ActiveCharacter = request.CharacterID;
            db.Sessions.Update(session);
            await db.SaveChangesAsync();
            return new SessionResponse(SessionResult.Success);
        }
        catch (Exception)
        {
            return new SessionResponse(SessionResult.FailedUnknown);
        }
    }

    public async Task<SessionGetOneResponse> GetByActiveAccount(SessionGetByActiveAccountRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var session = await db.Sessions.FirstOrDefaultAsync(s => s.ActiveAccount == request.AccountID);

            return session == null
                ? new SessionGetOneResponse(SessionResult.FailedNotFound)
                : new SessionGetOneResponse(SessionResult.Success, session.Adapt<Session>());
        }
        catch (Exception)
        {
            return new SessionGetOneResponse(SessionResult.FailedUnknown);
        }
    }

    public async Task<SessionGetOneResponse> GetByActiveCharacter(SessionGetByActiveCharacterRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var session = await db.Sessions.FirstOrDefaultAsync(s => s.ActiveCharacter == request.CharacterID);

            return session == null
                ? new SessionGetOneResponse(SessionResult.FailedNotFound)
                : new SessionGetOneResponse(SessionResult.Success, session.Adapt<Session>());
        }
        catch (Exception)
        {
            return new SessionGetOneResponse(SessionResult.FailedUnknown);
        }
    }
}
