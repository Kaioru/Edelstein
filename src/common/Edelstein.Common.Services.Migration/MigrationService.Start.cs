using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Database.Entities.Services.Migration;
using Edelstein.Protocol.Services.Migration.Contracts;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services.Migration;

public partial class MigrationService
{
    private static readonly TimeSpan Expiry = TimeSpan.FromMinutes(1);
    
    public async Task<MigrationServiceResponse> Start(MigrationServiceStartRequest request, CallContext context = default)
    {
        try
        {
            await using var db = await factory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var info = mapper.Map<DbMigrationInfo>(request.Info);
            
            await db.MigrationInfo
                .Where(i => i.AccountID == info.AccountID)
                .Where(i => i.DateExpire < now)
                .ExecuteDeleteAsync();
            
            info.DateUpdated = now;
            info.DateExpire = now.Add(Expiry);

            await db.AddAsync(info);
            await db.SaveChangesAsync();
            
            return new MigrationServiceResponse
            {
                Result = MigrationServiceResult.Success
            };
        }
        catch (DbException)
        {
            return new MigrationServiceResponse
            {
                Result = MigrationServiceResult.FailedUnknown
            };
        }
    }
}
