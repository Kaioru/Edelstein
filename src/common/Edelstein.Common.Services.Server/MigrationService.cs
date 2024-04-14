using AutoMapper;
using Edelstein.Common.Services.Server.Entities;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Migration.Contracts;
using Edelstein.Protocol.Services.Migration.Contracts.Requests;
using Edelstein.Protocol.Services.Migration.Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace Edelstein.Common.Services.Server;

public class MigrationService(
    IDbContextFactory<ServerDbContext> dbFactory, 
    IMapper mapper
) : IMigrationService
{
    private static readonly TimeSpan Expiry = TimeSpan.FromMinutes(1);
    
    public async Task<MigrationResponse> Start(MigrationStartRequest request, CallContext context = default) 
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Migrations
                .FirstOrDefaultAsync(m => m.AccountID == request.Migration.AccountID);

            if (existing != null)
            {
                if (existing.DateExpire < now) db.Migrations.Remove(existing);
                else
                    return new MigrationResponse
                    {
                        Result = MigrationResult.FailedAlreadyStarted
                    };
            }

            var entity = mapper.Map<MigrationEntity>(request.Migration);

            entity.DateUpdated = now;
            entity.DateExpire = now.Add(Expiry);

            db.Migrations.Add(entity);
            await db.SaveChangesAsync();
            return new MigrationResponse
            {
                Result = MigrationResult.Success
            };
        }
        catch (Exception)
        {
            return new MigrationResponse
            {
                Result = MigrationResult.FailedUnknown
            };
        }
    }
    
    public async Task<MigrationClaimResponse> Claim(MigrationClaimRequest request, CallContext context = default) 
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;
            var existing = await db.Migrations
                .FirstOrDefaultAsync(m => m.CharacterID == request.CharacterID);

            if (existing == null || existing.DateExpire < now)
                return new MigrationClaimResponse
                {
                    Result = MigrationResult.FailedNotStarted,
                    Migration = null
                };

            if (existing.Key != request.Key)
                return new MigrationClaimResponse
                {
                    Result = MigrationResult.FailedInvalidKey,
                    Migration = null
                };
            if (existing.ToServerID != request.ServerID)
                return new MigrationClaimResponse
                {
                    Result = MigrationResult.FailedInvalidServer,
                    Migration = null
                };

            db.Migrations.Remove(existing);
            await db.SaveChangesAsync();

            return new MigrationClaimResponse
            {
                Result = MigrationResult.FailedNotStarted,
                Migration = mapper.Map<MigrationEntry>(existing)
            };
        }
        catch (Exception)
        {
            return new MigrationClaimResponse
            {
                Result = MigrationResult.FailedUnknown,
                Migration = null
            };
        }
    }
}
