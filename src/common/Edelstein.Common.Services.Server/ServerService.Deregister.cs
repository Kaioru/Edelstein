using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Server.Contracts;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services.Server;

public partial class ServerService
{
    public async Task<ServerServiceResponse> Deregister(ServerServiceDeregisterRequest request, CallContext context = default)
    {
        try
        {
            await using var db = await factory.CreateDbContextAsync();
            var now = dateTimeProvider.Now;
            var count = await db.ServerInfo
                .Where(i => i.ID == request.ID)
                .Where(i => i.DateExpire > now)
                .Where(i => i.Secret == request.Secret)
                .ExecuteDeleteAsync();
            
            if (count == 0)
                return new ServerServiceResponse
                {
                    Result = ServerServiceResult.FailedNotRegistered
                };
            
            return new ServerServiceResponse
            {
                Result = ServerServiceResult.Success
            };
        }
        catch (DbException)
        {
            return new ServerServiceResponse
            {
                Result = ServerServiceResult.FailedUnknown
            };
        }
    }
}
