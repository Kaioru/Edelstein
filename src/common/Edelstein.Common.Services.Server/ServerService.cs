using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Database;
using Edelstein.Common.Database.Entities.Services.Server;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Utilities;
using EntityFramework.Exceptions.Common;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Server;

public partial class ServerService(
    IDbContextFactory<GameDbContext> factory,
    IMapper mapper,
    IDateTimeProvider dateTimeProvider
) : IServerService
{
    private static readonly TimeSpan Expiry = TimeSpan.FromMinutes(5);
    
    private async Task<ServerServiceRegisterResponse> Register<TServerInfo>(TServerInfo info) where TServerInfo : DbServerInfo
    {
        try
        {
            await using var db = await factory.CreateDbContextAsync();
            var now = dateTimeProvider.Now;

            await db.ServerInfo
                .Where(i => i.ID == info.ID)
                .Where(i => i.DateExpire < now)
                .ExecuteDeleteAsync();

            info.DateUpdated = now;
            info.DateExpire = now.Add(Expiry);
            info.Secret = Random.Shared.NextInt64();

            switch (info)
            {
                case DbServerInfoLogin login:
                    await db.ServerInfoLogin.AddAsync(login);
                    break;
                default:
                    await db.ServerInfo.AddAsync(info);
                    break;
            }

            await db.SaveChangesAsync();
            return new ServerServiceRegisterResponse
            {
                Result = ServerServiceResult.Success,
                Secret = info.Secret
            };
        }
        catch (UniqueConstraintException)
        {
            return new ServerServiceRegisterResponse
            {
                Result = ServerServiceResult.FailedAlreadyRegistered
            };
        }
        catch (DbException)
        {
            return new ServerServiceRegisterResponse
            {
                Result = ServerServiceResult.FailedUnknown
            };
        }
    }
}
