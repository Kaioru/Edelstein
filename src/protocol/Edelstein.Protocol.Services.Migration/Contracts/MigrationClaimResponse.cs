namespace Edelstein.Protocol.Services.Migration.Contracts;

public record MigrationClaimResponse(
    MigrationResult Result,
    IMigration? Migration = null
);
