namespace Edelstein.Protocol.Services.Migration.Contracts;

public record MigrationClaimRequest(
    int CharacterID,
    string ServerID,
    long Key
);
