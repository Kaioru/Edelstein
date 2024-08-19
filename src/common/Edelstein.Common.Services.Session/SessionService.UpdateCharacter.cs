using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Session.Contracts;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services.Session;

public partial class SessionService
{
    public async Task<SessionServiceResponse> UpdateCharacter(SessionServiceUpdateCharacterRequest request, CallContext context = default)
    {
        try
        {
            await using var db = await factory.CreateDbContextAsync();
            var count = await db.SessionInfo
                .Where(s => s.ActiveAccount == request.AccountID)
                .Where(s => s.Secret == request.Secret)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.ActiveCharacter, request.CharacterID));
            
            if (count == 0)
                return new SessionServiceResponse
                {
                    Result = SessionServiceResult.FailedNotFound
                };
            
            return new SessionServiceResponse
            {
                Result = SessionServiceResult.Success
            };
        }
        catch (DbException)
        {
            return new SessionServiceResponse
            {
                Result = SessionServiceResult.FailedUnknown
            };
        }
    }
}
