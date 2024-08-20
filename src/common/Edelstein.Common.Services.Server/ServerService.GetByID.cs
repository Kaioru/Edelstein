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
    public async Task<ServerServiceGetOneResponse<ServerInfo>> GetByID(ServerServiceGetByIDRequest request, CallContext context = default)
    {
        try
        {
            await using var db = await factory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var info = await db.ServerInfo
                .Where(i => i.ID == request.ID)
                .Where(i => i.DateExpire > now)
                .FirstAsync();
            
            return new ServerServiceGetOneResponse<ServerInfo>
            {
                Result = ServerServiceResult.Success,
                Info = mapper.Map<ServerInfo>(info)
            };
        }
        catch (DbException)
        {
            return new ServerServiceGetOneResponse<ServerInfo>
            {
                Result = ServerServiceResult.FailedUnknown
            };
        }
    }
}
