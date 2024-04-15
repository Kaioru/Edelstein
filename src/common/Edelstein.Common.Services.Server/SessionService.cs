using System;
using System.Threading.Tasks;
using AutoMapper;
using Edelstein.Common.Services.Server.Entities;
using Edelstein.Protocol.Services.Session;
using Edelstein.Protocol.Services.Session.Contracts;
using Edelstein.Protocol.Services.Session.Contracts.Requests;
using Edelstein.Protocol.Services.Session.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services.Server;

public class SessionService(
    IDbContextFactory<ServerDbContext> dbFactory, 
    IMapper mapper
) : ISessionService
{
    public async Task<SessionResponse> Start(SessionStartRequest request, CallContext context = default) 
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Sessions.FindAsync(request.Session.ActiveAccount);

            if (existing != null)
            {
                var migration = await db.Migrations
                    .FirstOrDefaultAsync(m => m.AccountID == request.Session.ActiveAccount);

                if (migration == null || migration.DateExpire > now)
                    return new SessionResponse
                    {
                        Result = SessionResult.FailedAlreadyStarted
                    };

                db.Sessions.Remove(existing);
                db.Migrations.Remove(migration);
            }

            var entity = mapper.Map<SessionEntity>(request.Session);

            entity.Key = request.Key;
            db.Sessions.Add(entity);
            await db.SaveChangesAsync();
            return new SessionResponse
            {
                Result = SessionResult.Success
            };
        }
        catch (Exception)
        {
            return new SessionResponse
            {
                Result = SessionResult.FailedUnknown
            };
        }
    }
    
    public async Task<SessionResponse> End(SessionEndRequest request, CallContext context = default) 
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var session = await db.Sessions.FindAsync(request.AccountID);

            if (session == null)
                return new SessionResponse
                {
                    Result = SessionResult.FailedNotStarted
                };
            if (session.Key != request.Key)
                return new SessionResponse
                {
                    Result = SessionResult.FailedInvalidKey
                };

            db.Sessions.Remove(session);
            await db.SaveChangesAsync();
            return new SessionResponse
            {
                Result = SessionResult.Success
            };
        }
        catch (Exception)
        {
            return new SessionResponse
            {
                Result = SessionResult.FailedUnknown
            };
        }
    }
    
    public async Task<SessionResponse> UpdateServer(SessionUpdateServerRequest request, CallContext context = default) 
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var session = await db.Sessions.FindAsync(request.ServerID);

            if (session == null)
                return new SessionResponse
                {
                    Result = SessionResult.FailedNotStarted
                };
            if (session.Key != request.Key)
                return new SessionResponse
                {
                    Result = SessionResult.FailedInvalidKey
                };

            session.ServerID = request.ServerID;
            db.Sessions.Update(session);
            await db.SaveChangesAsync();
            return new SessionResponse
            {
                Result = SessionResult.Success
            };
        }
        catch (Exception)
        {
            return new SessionResponse
            {
                Result = SessionResult.FailedUnknown
            };
        }
    }
    
    public async Task<SessionResponse> UpdateCharacter(SessionUpdateCharacterRequest request, CallContext context = default) 
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var session = await db.Sessions.FindAsync(request.AccountID);

            if (session == null)
                return new SessionResponse
                {
                    Result = SessionResult.FailedNotStarted
                };
            if (session.Key != request.Key)
                return new SessionResponse
                {
                    Result = SessionResult.FailedInvalidKey
                };

            session.ActiveCharacter = request.CharacterID;
            db.Sessions.Update(session);
            await db.SaveChangesAsync();
            return new SessionResponse
            {
                Result = SessionResult.Success
            };
        }
        catch (Exception)
        {
            return new SessionResponse
            {
                Result = SessionResult.FailedUnknown
            };
        }
    }
    
    public async Task<SessionGetOneResponse> GetByActiveAccount(SessionGetByActiveAccountRequest request, CallContext context = default) 
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var session = await db.Sessions.FirstOrDefaultAsync(s => s.ActiveAccount == request.AccountID);

            return session == null
                ? new SessionGetOneResponse
                {
                    Result = SessionResult.FailedNotFound,
                    Session = null
                }
                : new SessionGetOneResponse
                {
                    Result = SessionResult.Success,
                    Session = mapper.Map<SessionEntry>(session)
                };
        }
        catch (Exception)
        {
            return new SessionGetOneResponse
            {
                Result = SessionResult.FailedUnknown,
                Session = null
            };
        }
    }
    
    public async Task<SessionGetOneResponse> GetByActiveCharacter(SessionGetByActiveCharacterRequest request, CallContext context = default) 
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var session = await db.Sessions.FirstOrDefaultAsync(s => s.ActiveCharacter == request.CharacterID);

            return session == null
                ? new SessionGetOneResponse
                {
                    Result = SessionResult.FailedNotFound,
                    Session = null
                }
                : new SessionGetOneResponse
                {
                    Result = SessionResult.Success,
                    Session = mapper.Map<SessionEntry>(session)
                };
        }
        catch (Exception)
        {
            return new SessionGetOneResponse
            {
                Result = SessionResult.FailedUnknown,
                Session = null
            };
        }
    }
}
