using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Common.Services.Server.Models;
using Edelstein.Common.Services.Server.Types;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Migration.Contracts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Server;

public class MigrationService : IMigrationService
{
    private static readonly TimeSpan _expiry = TimeSpan.FromMinutes(5);
    private readonly IDbContextFactory<ServerDbContext> _dbFactory;

    public MigrationService(IDbContextFactory<ServerDbContext> dbFactory) => _dbFactory = dbFactory;

    public async Task<IMigrationResponse> Start(IMigrationStartRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Migrations.FindAsync(request.Migration.ID);

            if (existing != null)
            {
                if (existing.DateExpire < now) db.Migrations.Remove(existing);
                else return new MigrationResponse(MigrationResult.FailedAlreadyStarted);
            }

            var model = request.Migration.Adapt<MigrationModel>();

            model.DateUpdated = now;
            model.DateExpire = now.Add(_expiry);

            db.Migrations.Add(model);
            await db.SaveChangesAsync();
            return new MigrationResponse(MigrationResult.Success);
        }
        catch (Exception)
        {
            return new MigrationResponse(MigrationResult.FailedUnknown);
        }
    }

    public async Task<IMigrationClaimResponse> Claim(IMigrationClaimRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Migrations.FindAsync(request.ID);

            if (existing == null || existing.DateExpire < now)
                return new MigrationClaimResponse(MigrationResult.FailedNotStarted);

            db.Migrations.Remove(existing);
            await db.SaveChangesAsync();

            return new MigrationClaimResponse(MigrationResult.Success, existing.Adapt<Migration>());
        }
        catch (Exception)
        {
            return new MigrationClaimResponse(MigrationResult.FailedUnknown);
        }
    }
}
