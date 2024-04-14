using System.Collections.Immutable;
using AutoMapper;
using Edelstein.Common.Services.Server.Entities;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Services.Server.Contracts.Requests;
using Edelstein.Protocol.Services.Server.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services.Server;

public class ServerService(
    IDbContextFactory<ServerDbContext> dbFactory, 
    IMapper mapper
) : IServerService
{
    private static readonly TimeSpan Expiry = TimeSpan.FromMinutes(5);

    public Task<ServerRegisterResponse> RegisterLogin(ServerRegisterRequest<ServerEntryLogin> request, CallContext context = default) 
        => Register(mapper.Map<ServerEntityLogin>(request.Server));

    public Task<ServerRegisterResponse> RegisterGame(ServerRegisterRequest<ServerEntryGame> request, CallContext context = default) 
        => Register(mapper.Map<ServerEntityGame>(request.Server));
    
    public Task<ServerRegisterResponse> RegisterShop(ServerRegisterRequest<ServerEntryShop> request, CallContext context = default) 
        => Register(mapper.Map<ServerEntityShop>(request.Server));
    
    public Task<ServerRegisterResponse> RegisterTrade(ServerRegisterRequest<ServerEntryTrade> request, CallContext context = default) 
        => Register(mapper.Map<ServerEntityTrade>(request.Server));

    public async Task<ServerResponse> Update(ServerUpdateRequest request, CallContext context = default)
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Servers.FirstOrDefaultAsync(s => s.ID.Equals(request.ServerID));

            if (existing == null || existing.DateExpire < now)
                return new ServerResponse
                {
                    Result = ServerResult.FailedNotRegistered
                };
            
            if (existing.Token != request.Token)
                return new ServerResponse
                {
                    Result = ServerResult.FailedInvalidToken
                };

            existing.DateUpdated = now;
            existing.DateExpire = now.Add(Expiry);

            db.Servers.Update(existing);
            await db.SaveChangesAsync();

            return new ServerResponse
            {
                Result = ServerResult.Success
            };
        }
        catch (Exception)
        {
            return new ServerResponse
            {
                Result = ServerResult.FailedUnknown
            };
        }
    }
    
    public async Task<ServerResponse> Deregister(ServerDeregisterRequest request, CallContext context = default)
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var existing = await db.Servers.FindAsync(request.ServerID);

            if (existing == null)
                return new ServerResponse
                {
                    Result = ServerResult.FailedNotRegistered
                };
            
            if (existing.Token != request.Token)
                return new ServerResponse
                {
                    Result = ServerResult.FailedInvalidToken
                };

            db.Servers.Remove(existing);
            await db.SaveChangesAsync();
            return new ServerResponse
            {
                Result = ServerResult.Success
            };
        }
        catch (Exception)
        {
            return new ServerResponse
            {
                Result = ServerResult.FailedUnknown
            };
        }
    }
    
    public async Task<ServerGetOneResponse<ServerEntry>> GetByID(ServerGetByIDRequest request, CallContext context = default) 
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Servers.FirstOrDefaultAsync(s => s.ID.Equals(request.ServerID));

            if (existing == null || existing.DateExpire < now)
                return new ServerGetOneResponse<ServerEntry>
                {
                    Result = ServerResult.FailedNotFound,
                    Server = null
                };

            return new ServerGetOneResponse<ServerEntry>
            {
                Result = ServerResult.Success,
                Server = mapper.Map<ServerEntry>(existing)
            };
        }
        catch (Exception)
        {
            return new ServerGetOneResponse<ServerEntry>
            {
                Result = ServerResult.FailedUnknown,
                Server = null
            };
        }
    }
    
    public async Task<ServerGetOneResponse<ServerEntryGame>> GetGameByWorldAndChannel(ServerGetGameByWorldAndChannelRequest request, CallContext context = default) 
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.GameServers
                .FirstOrDefaultAsync(s => s.WorldID == request.WorldID && s.ChannelID == request.ChannelID);

            if (existing == null || existing.DateExpire < now)
                return new ServerGetOneResponse<ServerEntryGame>
                {
                    Result = ServerResult.FailedNotFound,
                    Server = null
                };

            return new ServerGetOneResponse<ServerEntryGame>
            {
                Result = ServerResult.Success,
                Server = mapper.Map<ServerEntryGame>(existing)
            };
        }
        catch (Exception)
        {
            return new ServerGetOneResponse<ServerEntryGame>
            {
                Result = ServerResult.FailedUnknown,
                Server = null
            };
        }
    }
    
    public async Task<ServerGetAllResponse<ServerEntryGame>> GetGameByWorld(ServerGetGameByWorldRequest request, CallContext context = default) 
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.GameServers
                .Where(s => s.WorldID == request.WorldID)
                .ToListAsync();

            return new ServerGetAllResponse<ServerEntryGame>
            {
                Result = ServerResult.Success,
                Servers = existing
                    .Where(s => s.DateExpire > now)
                    .Select(mapper.Map<ServerEntryGame>)
                    .ToImmutableArray()
            };
        }
        catch (Exception)
        {
            return new ServerGetAllResponse<ServerEntryGame>
            {
                Result = ServerResult.FailedUnknown,
                Servers = Enumerable.Empty<ServerEntryGame>()
            };
        }
    }
    
    public async Task<ServerGetOneResponse<ServerEntryShop>> GetShopByWorld(ServerGetShopByWorldRequest request, CallContext context = default) 
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.ShopServers
                .FirstOrDefaultAsync(s => s.WorldID == request.WorldID);

            if (existing == null || existing.DateExpire < now)
                return new ServerGetOneResponse<ServerEntryShop>
                {
                    Result = ServerResult.FailedNotFound,
                    Server = null
                };

            return new ServerGetOneResponse<ServerEntryShop>
            {
                Result = ServerResult.Success,
                Server = mapper.Map<ServerEntryShop>(existing)
            };
        }
        catch (Exception)
        {
            return new ServerGetOneResponse<ServerEntryShop>
            {
                Result = ServerResult.FailedUnknown,
                Server = null
            };
        }
    }
    
    public async Task<ServerGetOneResponse<ServerEntryTrade>> GetTradeByWorld(ServerGetTradeByWorldRequest request, CallContext context = default)
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.TradeServers
                .FirstOrDefaultAsync(s => s.WorldID == request.WorldID);

            if (existing == null || existing.DateExpire < now)
                return new ServerGetOneResponse<ServerEntryTrade>
                {
                    Result = ServerResult.FailedNotFound,
                    Server = null
                };

            return new ServerGetOneResponse<ServerEntryTrade>
            {
                Result = ServerResult.Success,
                Server = mapper.Map<ServerEntryTrade>(existing)
            };
        }
        catch (Exception)
        {
            return new ServerGetOneResponse<ServerEntryTrade>
            {
                Result = ServerResult.FailedUnknown,
                Server = null
            };
        }
    }
    
    public async Task<ServerGetAllResponse<ServerEntry>> GetAll(CallContext context = default) 
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Servers.ToListAsync();
            
            return new ServerGetAllResponse<ServerEntry>
            {
                Result = ServerResult.Success,
                Servers = existing
                    .Where(s => s.DateExpire > now)
                    .Select(mapper.Map<ServerEntry>)
                    .ToImmutableArray()
            };
        }
        catch (Exception)
        {
            return new ServerGetAllResponse<ServerEntry>
            {
                Result = ServerResult.Unknown,
                Servers = Enumerable.Empty<ServerEntry>()
            };
        }
    }
    
    private async Task<ServerRegisterResponse> Register(ServerEntity entity)
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Servers.FindAsync(entity.ID);

            if (existing != null)
            {
                if (existing.DateExpire < now)
                {
                    db.Servers.Remove(existing);
                    await db.SaveChangesAsync();
                }
                else
                    return new ServerRegisterResponse
                    {
                        Result = ServerResult.FailedAlreadyRegistered,
                        Token = null
                    };
            }

            entity.Token = new Random().NextInt64();
            entity.DateUpdated = now;
            entity.DateExpire = now.Add(Expiry);

            switch (entity)
            {
                case ServerEntityLogin login:
                    await db.LoginServers.AddAsync(login);
                    break;
                case ServerEntityGame game:
                    if (db.GameServers.Any(s => s.WorldID == game.WorldID && s.ChannelID == game.ChannelID))
                        return new ServerRegisterResponse
                        {
                            Result = ServerResult.FailedAlreadyRegistered,
                            Token = null
                        };
                    await db.GameServers.AddAsync(game);
                    break;
                case ServerEntityShop shop:
                    if (db.ShopServers.Any(s => s.WorldID == shop.WorldID))
                        return new ServerRegisterResponse
                        {
                            Result = ServerResult.FailedAlreadyRegistered,
                            Token = null
                        };
                    await db.ShopServers.AddAsync(shop);
                    break;
                case ServerEntityTrade trade:
                    if (db.TradeServers.Any(s => s.WorldID == trade.WorldID))
                        return new ServerRegisterResponse
                        {
                            Result = ServerResult.FailedAlreadyRegistered,
                            Token = null
                        };
                    await db.TradeServers.AddAsync(trade);
                    break;
                default:
                    await db.Servers.AddAsync(entity);
                    break;
            }

            await db.SaveChangesAsync();
            return new ServerRegisterResponse
            {
                Result = ServerResult.Success,
                Token = entity.Token
            };
        }
        catch (Exception)
        {
            return new ServerRegisterResponse
            {
                Result = ServerResult.FailedUnknown,
                Token = null
            };
        }
    }
}
