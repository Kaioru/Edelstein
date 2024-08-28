using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Server.Contracts;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services.Server;

public partial class ServerService
{
    public async Task<ServerServiceGetOneResponse<ServerInfoGame>> GetGameByWorldAndChannel(ServerServiceGetByWorldAndChannelRequest request, CallContext context = default)
    {
        try
        {
            await using var db = await factory.CreateDbContextAsync();
            var now = dateTimeProvider.Now;
            var info = await db.ServerInfoGame
                .Where(i => i.DateExpire > now)
                .Where(i => i.WorldID == request.WorldID)
                .Where(i => i.ChannelID == request.ChannelID)
                .FirstAsync();
            
            return new ServerServiceGetOneResponse<ServerInfoGame>
            {
                Result = ServerServiceResult.Success,
                Info = mapper.Map<ServerInfoGame>(info)
            };
        }
        catch (DbException)
        {
            return new ServerServiceGetOneResponse<ServerInfoGame>
            {
                Result = ServerServiceResult.FailedUnknown
            };
        }
    }
}
