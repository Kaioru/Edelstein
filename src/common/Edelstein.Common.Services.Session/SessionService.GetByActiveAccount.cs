using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Session.Contracts;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services.Session;

public partial class SessionService
{
    public async Task<SessionServiceGetOneResponse> GetByActiveAccount(SessionServiceGetByActiveAccountRequest request, CallContext context = default)
    {
        try
        {
            await using var db = await factory.CreateDbContextAsync();
            var info = await db.SessionInfo
                .Where(i => i.ActiveAccount == request.AccountID)
                .FirstAsync();
            
            return new SessionServiceGetOneResponse
            {
                Result = SessionServiceResult.Success,
                Info = mapper.Map<SessionInfo>(info)
            };
        }
        catch (DbException)
        {
            return new SessionServiceGetOneResponse
            {
                Result = SessionServiceResult.FailedUnknown
            };
        }
    }
}
