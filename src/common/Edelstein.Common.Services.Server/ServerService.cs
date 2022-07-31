using System.Collections.Immutable;
using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Common.Services.Server.Models;
using Edelstein.Common.Services.Server.Types;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Services.Server.Types;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Server;

public class ServerService : IServerService
{
    private static readonly TimeSpan _expiry = TimeSpan.FromMinutes(5);
    private readonly IDbContextFactory<ServerDbContext> _dbFactory;

    public ServerService(IDbContextFactory<ServerDbContext> dbFactory) => _dbFactory = dbFactory;

    public Task<IServerResponse> RegisterLogin(IServerRegisterRequest<IServerLogin> request) =>
        Register(request.Server.Adapt<ServerLoginModel>());

    public Task<IServerResponse> RegisterGame(IServerRegisterRequest<IServerGame> request) =>
        Register(request.Server.Adapt<ServerGameModel>());

    public async Task<IServerResponse> Ping(IServerPingRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Servers.FirstOrDefaultAsync(s => s.ID.Equals(request.ID));

            if (existing == null || existing.DateExpire < now)
                return new ServerResponse(ServerResult.FailedNotRegistered);

            existing.DateUpdated = now;
            existing.DateExpire = now.Add(_expiry);

            db.Servers.Update(existing);
            await db.SaveChangesAsync();

            return new ServerResponse(ServerResult.Success);
        }
        catch (Exception)
        {
            return new ServerResponse(ServerResult.FailedUnknown);
        }
    }

    public async Task<IServerResponse> Deregister(IServerDeregisterRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var existing = await db.Servers.FindAsync(request.ID);

            if (existing == null)
                return new ServerResponse(ServerResult.FailedNotRegistered);

            db.Servers.Remove(existing);
            await db.SaveChangesAsync();
            return new ServerResponse(ServerResult.Success);
        }
        catch (Exception)
        {
            return new ServerResponse(ServerResult.FailedUnknown);
        }
    }

    public async Task<IServerGetOneResponse<IServer>> GetByID(IServerGetByIDRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Servers.FirstOrDefaultAsync(s => s.ID.Equals(request.ID));

            if (existing == null || existing.DateExpire < now)
                return new ServerGetOneResponse<IServer>(ServerResult.FailedNotFound);

            return new ServerGetOneResponse<IServer>(ServerResult.Success, existing.Adapt<Types.Server>());
        }
        catch (Exception)
        {
            return new ServerGetOneResponse<IServer>(ServerResult.FailedUnknown);
        }
    }

    public async Task<IServerGetOneResponse<IServerGame>> GetGameByWorldAndChannel(
        IServerGetGameByWorldAndChannelRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.GameServers
                .FirstOrDefaultAsync(s => s.WorldID == request.WorldID && s.ChannelID == request.ChannelID);

            if (existing == null || existing.DateExpire < now)
                return new ServerGetOneResponse<IServerGame>(ServerResult.FailedNotFound);

            return new ServerGetOneResponse<IServerGame>(ServerResult.Success, existing.Adapt<ServerGame>());
        }
        catch (Exception)
        {
            return new ServerGetOneResponse<IServerGame>(ServerResult.FailedUnknown);
        }
    }

    public async Task<IServerGetAllResponse<IServerGame>> GetGameByWorld(IServerGetGameByWorldRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.GameServers
                .Where(s => s.WorldID == request.WorldID)
                .ToListAsync();

            return new ServerGetAllResponse<IServerGame>(ServerResult.Success, existing
                .Where(s => s.DateExpire > now)
                .Select(s => s.Adapt<ServerGame>())
                .ToImmutableList());
        }
        catch (Exception)
        {
            return new ServerGetAllResponse<IServerGame>(ServerResult.FailedUnknown, Enumerable.Empty<IServerGame>());
        }
    }

    public async Task<IServerGetAllResponse<IServer>> GetAll()
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Servers.ToListAsync();

            return new ServerGetAllResponse<IServer>(ServerResult.Success, existing
                .Where(s => s.DateExpire > now)
                .Select(s => s.Adapt<Types.Server>())
                .ToImmutableList());
        }
        catch (Exception)
        {
            return new ServerGetAllResponse<IServer>(ServerResult.FailedUnknown, Enumerable.Empty<IServer>());
        }
    }

    private async Task<IServerResponse> Register(ServerModel model)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Servers.FindAsync(model.ID);

            if (existing != null)
            {
                if (existing.DateExpire < now) db.Servers.Remove(existing);
                else return new ServerResponse(ServerResult.FailedAlreadyRegistered);
            }

            model.DateUpdated = now;
            model.DateExpire = now.Add(_expiry);

            switch (model)
            {
                case ServerLoginModel login:
                    await db.LoginServers.AddAsync(login);
                    break;
                case ServerGameModel game:
                    if (db.GameServers.Any(s => s.WorldID == game.WorldID && s.ChannelID == game.ChannelID))
                        return new ServerResponse(ServerResult.FailedAlreadyRegistered);

                    await db.GameServers.AddAsync(game);
                    break;
                default:
                    await db.Servers.AddAsync(model);
                    break;
            }

            await db.SaveChangesAsync();
            return new ServerResponse(ServerResult.Success);
        }
        catch (Exception)
        {
            return new ServerResponse(ServerResult.FailedUnknown);
        }
    }
}
