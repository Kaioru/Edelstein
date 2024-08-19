using System;
using System.Data.Common;
using System.Threading.Tasks;
using Edelstein.Common.Database.Entities.Services.Session;
using Edelstein.Protocol.Services.Session.Contracts;
using EntityFramework.Exceptions.Common;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services.Session;

public partial class SessionService
{
    public async Task<SessionServiceStartResponse> Start(SessionServiceStartRequest request, CallContext context = default)
    {
        try
        {
            await using var db = await factory.CreateDbContextAsync();
            var info = mapper.Map<DbSessionInfo>(request.Info);

            info.Secret = Random.Shared.NextInt64();

            await db.SessionInfo.AddAsync(info);
            await db.SaveChangesAsync();

            return new SessionServiceStartResponse
            {
                Result = SessionServiceResult.Success,
                Secret = info.Secret
            };
        }
        catch (UniqueConstraintException)
        {
            return new SessionServiceStartResponse
            {
                Result = SessionServiceResult.FailedAlreadyStarted
            };
        }
        catch (DbException)
        {
            return new SessionServiceStartResponse
            {
                Result = SessionServiceResult.FailedUnknown
            };
        }
    }
}
