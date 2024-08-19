using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Server.Contracts;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services.Server;

public partial class ServerService
{
    public async Task<ServerServiceGetAllResponse<ServerServiceServerInfo>> GetAll(CallContext context = default)
    {
        try
        {
            await using var db = await factory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var info = await db.ServerInfo
                .Where(i => i.DateExpire < now)
                .ToHashSetAsync();
            
            return new ServerServiceGetAllResponse<ServerServiceServerInfo>
            {
                Result = ServerServiceResult.Success,
                Info = info.Select(mapper.Map<ServerServiceServerInfo>).ToHashSet()
            };
        }
        catch (DbException)
        {
            return new ServerServiceGetAllResponse<ServerServiceServerInfo>
            {
                Result = ServerServiceResult.FailedUnknown
            };
        }
    }
}
