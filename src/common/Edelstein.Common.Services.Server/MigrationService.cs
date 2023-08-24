using AutoMapper;
using Edelstein.Common.Services.Server.Entities;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Migration.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.Common.Services.Server;

public class MigrationService : IMigrationService
{
    private static readonly TimeSpan Expiry = TimeSpan.FromMinutes(1);
    private readonly IDbContextFactory<ServerDbContext> _dbFactory;
    private readonly IMapper _mapper;

    public MigrationService(IDbContextFactory<ServerDbContext> dbFactory, IMapper mapper)
    {
        _dbFactory = dbFactory;
        _mapper = mapper;
    }

    public async Task<MigrationResponse> Start(MigrationStartRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Migrations
                .FirstOrDefaultAsync(m => m.AccountID == request.Migration.AccountID);

            if (existing != null)
            {
                if (existing.DateExpire < now) db.Migrations.Remove(existing);
                else return new MigrationResponse(MigrationResult.FailedAlreadyStarted);
            }

            var entity = _mapper.Map<MigrationEntity>(request.Migration);

            entity.DateUpdated = now;
            entity.DateExpire = now.Add(Expiry);

            db.Migrations.Add(entity);
            await db.SaveChangesAsync();
            return new MigrationResponse(MigrationResult.Success);
        }
        catch (Exception)
        {
            return new MigrationResponse(MigrationResult.FailedUnknown);
        }
    }

    public async Task<MigrationClaimResponse> Claim(MigrationClaimRequest request)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Migrations
                .FirstOrDefaultAsync(m => m.CharacterID == request.CharacterID);

            if (existing == null || existing.DateExpire < now)
                return new MigrationClaimResponse(MigrationResult.FailedNotStarted);

            if (existing.Key != request.Key)
                return new MigrationClaimResponse(MigrationResult.FailedInvalidKey);
            if (existing.ToServerID != request.ServerID)
                return new MigrationClaimResponse(MigrationResult.FailedInvalidServer);

            db.Migrations.Remove(existing);
            await db.SaveChangesAsync();

            var migration = _mapper.Map<Migration>(existing);

            return new MigrationClaimResponse(MigrationResult.Success, migration);
        }
        catch (Exception)
        {
            return new MigrationClaimResponse(MigrationResult.FailedUnknown);
        }
    }
}
