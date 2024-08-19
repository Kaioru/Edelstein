using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Migration.Contracts;
using Edelstein.Protocol.Services.Migration.Entities;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services.Migration;

public partial class MigrationService
{
    public async Task<MigrationServiceClaimResponse> Claim(MigrationServiceClaimRequest request, CallContext context = default)
    {
        try
        {
            await using var db = await factory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var info = await db.MigrationInfo
                .Include(i => i.Session)
                .Where(i => i.CharacterID == request.CharacterID)
                .Where(i => i.Session.Secret == request.Secret)
                .Where(i => i.DateExpire > now)
                .FirstAsync();

            db.Remove(info);
            await db.SaveChangesAsync();
            
            return new MigrationServiceClaimResponse
            {
                Result = MigrationServiceResult.Success,
                Info = mapper.Map<MigrationServiceMigrationInfo>(info)
            };
        }
        catch (DbException)
        {
            return new MigrationServiceClaimResponse
            {
                Result = MigrationServiceResult.FailedUnknown
            };
        }
    }
}
