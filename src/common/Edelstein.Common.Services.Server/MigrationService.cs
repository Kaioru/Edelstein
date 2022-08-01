using Edelstein.Common.Services.Server.Contracts;
using Edelstein.Common.Services.Server.Models;
using Edelstein.Common.Services.Server.Types;
using Edelstein.Common.Util.Serializers;
using Edelstein.Protocol.Gameplay.Accounts;
using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Migration.Contracts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Server;

public class MigrationService : IMigrationService
{
    private static readonly TimeSpan _expiry = TimeSpan.FromMinutes(5);
    private readonly IDbContextFactory<ServerDbContext> _dbFactory;
    private readonly ISerializer _serializer;

    public MigrationService(IDbContextFactory<ServerDbContext> dbFactory, ISerializer serializer)
    {
        _dbFactory = dbFactory;
        _serializer = serializer;
    }

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

            model.AccountBytes = _serializer.Serialize(request.Migration.Account);
            model.AccountWorldBytes = _serializer.Serialize(request.Migration.AccountWorld);
            model.CharacterBytes = _serializer.Serialize(request.Migration.Character);

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

            if (existing.Key != request.Key)
                return new MigrationClaimResponse(MigrationResult.FailedInvalidKey);
            if (existing.ToServerID != request.ServerID)
                return new MigrationClaimResponse(MigrationResult.FailedInvalidServer);

            db.Migrations.Remove(existing);
            await db.SaveChangesAsync();

            var migration = existing.Adapt<Migration>();

            migration.Account = _serializer.Deserialize<IAccount>(existing.AccountBytes);
            migration.AccountWorld = _serializer.Deserialize<IAccountWorld>(existing.AccountWorldBytes);
            migration.Character = _serializer.Deserialize<ICharacter>(existing.CharacterBytes);

            return new MigrationClaimResponse(MigrationResult.Success, existing.Adapt<Migration>());
        }
        catch (Exception)
        {
            return new MigrationClaimResponse(MigrationResult.FailedUnknown);
        }
    }
}
