using System.Collections.Immutable;
using AutoMapper;
using Edelstein.Common.Services.Server.Entities;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Server;

public class ServerService : IServerService
{
    private static readonly TimeSpan Expiry = TimeSpan.FromMinutes(5);
    private readonly IDbContextFactory<ServerDbContext> _dbFactory;
    private readonly IMapper _mapper;

    public ServerService(IDbContextFactory<ServerDbContext> dbFactory, IMapper mapper)
    {
        _dbFactory = dbFactory;
        _mapper = mapper;
    }

    public Task<ServerResponse> RegisterLogin(ServerRegisterRequest<IServerLogin> request) =>
        Register(_mapper.Map<ServerLoginEntity>(request.Server));

    public Task<ServerResponse> RegisterGame(ServerRegisterRequest<IServerGame> request) =>
        Register(_mapper.Map<ServerGameEntity>(request.Server));
    
    public Task<ServerResponse> RegisterShop(ServerRegisterRequest<IServerShop> request) =>
        Register(_mapper.Map<ServerShopEntity>(request.Server));
    
    public Task<ServerResponse> RegisterTrade(ServerRegisterRequest<IServerTrade> request) =>
        Register(_mapper.Map<ServerTradeEntity>(request.Server));

    public async Task<ServerResponse> Ping(ServerPingRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Servers.FirstOrDefaultAsync(s => s.ID.Equals(request.ID));

            if (existing == null || existing.DateExpire < now)
                return new ServerResponse(ServerResult.FailedNotRegistered);

            existing.DateUpdated = now;
            existing.DateExpire = now.Add(Expiry);

            db.Servers.Update(existing);
            await db.SaveChangesAsync();

            return new ServerResponse(ServerResult.Success);
        }
        catch (Exception)
        {
            return new ServerResponse(ServerResult.FailedUnknown);
        }
    }

    public async Task<ServerResponse> Deregister(ServerDeregisterRequest request)
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

    public async Task<ServerGetOneResponse<IServer>> GetByID(ServerGetByIDRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Servers.FirstOrDefaultAsync(s => s.ID.Equals(request.ID));

            if (existing == null || existing.DateExpire < now)
                return new ServerGetOneResponse<IServer>(ServerResult.FailedNotFound);

            return new ServerGetOneResponse<IServer>(ServerResult.Success, _mapper.Map<Protocol.Services.Server.Contracts.Server>(existing));
        }
        catch (Exception)
        {
            return new ServerGetOneResponse<IServer>(ServerResult.FailedUnknown);
        }
    }

    public async Task<ServerGetOneResponse<IServerGame>> GetGameByWorldAndChannel(
        ServerGetGameByWorldAndChannelRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.GameServers
                .FirstOrDefaultAsync(s => s.WorldID == request.WorldID && s.ChannelID == request.ChannelID);

            if (existing == null || existing.DateExpire < now)
                return new ServerGetOneResponse<IServerGame>(ServerResult.FailedNotFound);

            return new ServerGetOneResponse<IServerGame>(ServerResult.Success, _mapper.Map<ServerGame>(existing));
        }
        catch (Exception)
        {
            return new ServerGetOneResponse<IServerGame>(ServerResult.FailedUnknown);
        }
    }

    public async Task<ServerGetAllResponse<IServerGame>> GetGameByWorld(ServerGetGameByWorldRequest request)
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
                .Select(s => _mapper.Map<ServerGame>(s))
                .ToImmutableList());
        }
        catch (Exception)
        {
            return new ServerGetAllResponse<IServerGame>(ServerResult.FailedUnknown, Enumerable.Empty<IServerGame>());
        }
    }

    public async Task<ServerGetOneResponse<IServerShop>> GetShopByWorld(ServerGetShopByWorldRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.ShopServers
                .FirstOrDefaultAsync(s => s.WorldID == request.WorldID);

            if (existing == null || existing.DateExpire < now)
                return new ServerGetOneResponse<IServerShop>(ServerResult.FailedNotFound);

            return new ServerGetOneResponse<IServerShop>(ServerResult.Success, _mapper.Map<ServerShop>(existing));
        }
        catch (Exception)
        {
            return new ServerGetOneResponse<IServerShop>(ServerResult.FailedUnknown);
        }
    }
    
    public async Task<ServerGetOneResponse<IServerTrade>> GetTradeByWorld(ServerGetTradeByWorldRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.TradeServers
                .FirstOrDefaultAsync(s => s.WorldID == request.WorldID);

            if (existing == null || existing.DateExpire < now)
                return new ServerGetOneResponse<IServerTrade>(ServerResult.FailedNotFound);

            return new ServerGetOneResponse<IServerTrade>(ServerResult.Success, _mapper.Map<ServerTrade>(existing));
        }
        catch (Exception)
        {
            return new ServerGetOneResponse<IServerTrade>(ServerResult.FailedUnknown);
        }
    }

    public async Task<ServerGetAllResponse<IServer>> GetAll()
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Servers.ToListAsync();

            return new ServerGetAllResponse<IServer>(ServerResult.Success, existing
                .Where(s => s.DateExpire > now)
                .Select(s => _mapper.Map<Protocol.Services.Server.Contracts.Server>(s))
                .ToImmutableList());
        }
        catch (Exception)
        {
            return new ServerGetAllResponse<IServer>(ServerResult.FailedUnknown, Enumerable.Empty<IServer>());
        }
    }

    private async Task<ServerResponse> Register(ServerEntity entity)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Servers.FindAsync(entity.ID);

            if (existing != null)
            {
                if (existing.DateExpire < now) db.Servers.Remove(existing);
                else return new ServerResponse(ServerResult.FailedAlreadyRegistered);
            }

            entity.DateUpdated = now;
            entity.DateExpire = now.Add(Expiry);

            switch (entity)
            {
                case ServerLoginEntity login:
                    await db.LoginServers.AddAsync(login);
                    break;
                case ServerGameEntity game:
                    if (db.GameServers.Any(s => s.WorldID == game.WorldID && s.ChannelID == game.ChannelID))
                        return new ServerResponse(ServerResult.FailedAlreadyRegistered);
                    await db.GameServers.AddAsync(game);
                    break;
                case ServerShopEntity shop:
                    if (db.ShopServers.Any(s => s.WorldID == shop.WorldID))
                        return new ServerResponse(ServerResult.FailedAlreadyRegistered);
                    await db.ShopServers.AddAsync(shop);
                    break;
                default:
                    await db.Servers.AddAsync(entity);
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
