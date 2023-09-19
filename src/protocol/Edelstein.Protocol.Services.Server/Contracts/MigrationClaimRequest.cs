namespace Edelstein.Protocol.Services.Server.Contracts;

public record MigrationClaimRequest(
    int CharacterID,
    string ServerID,
    long Key
);
