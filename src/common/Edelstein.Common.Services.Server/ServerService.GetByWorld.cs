using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Server.Contracts;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services.Server;

public partial class ServerService
{
    public async Task<ServerServiceGetAllResponse<ServerInfoGame>> GetGameByWorld(ServerServiceGetByWorldRequest request, CallContext context = default)
    {
        try
        {
            await using var db = await factory.CreateDbContextAsync();
            var now = dateTimeProvider.Now;
            var info = await db.ServerInfoGame
                .Where(i => i.DateExpire > now)
                .Where(i => i.WorldID == request.WorldID)
                .ToHashSetAsync();
            
            return new ServerServiceGetAllResponse<ServerInfoGame>
            {
                Result = ServerServiceResult.Success,
                Info = info.Select(mapper.Map<ServerInfoGame>).ToHashSet()
            };
        }
        catch (DbException)
        {
            return new ServerServiceGetAllResponse<ServerInfoGame>
            {
                Result = ServerServiceResult.FailedUnknown
            };
        }
    }
}
