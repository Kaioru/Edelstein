namespace Edelstein.Protocol.Services.Server.Contracts;

public record MigrationClaimResponse(
    MigrationResult Result,
    IMigration? Migration = null
);
