using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Common.Services.Server.Models;
using Edelstein.Common.Services.Server.Types;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Services.Session.Contracts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Server;

public class SessionService : ISessionService
{
    private readonly IDbContextFactory<ServerDbContext> _dbFactory;

    public SessionService(IDbContextFactory<ServerDbContext> dbFactory) => _dbFactory = dbFactory;

    public async Task<ISessionResponse> Start(ISessionStartRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();

            if (await db.Sessions.FindAsync(request.Session.ActiveAccount) != null)
                return new SessionResponse(SessionResult.FailedAlreadyStarted);

            db.Sessions.Add(request.Session.Adapt<SessionModel>());
            await db.SaveChangesAsync();
            return new SessionResponse(SessionResult.Success);
        }
        catch (Exception)
        {
            return new SessionResponse(SessionResult.FailedUnknown);
        }
    }

    public async Task<ISessionResponse> End(ISessionEndRequest request)
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

    public async Task<ISessionResponse> UpdateServer(ISessionUpdateServerRequest request)
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

    public async Task<ISessionResponse> UpdateCharacter(ISessionUpdateCharacterRequest request)
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

    public async Task<ISessionGetOneResponse> GetByActiveAccount(ISessionGetByActiveAccountRequest request)
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

    public async Task<ISessionGetOneResponse> GetByActiveCharacter(ISessionGetByActiveCharacterRequest request)
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
