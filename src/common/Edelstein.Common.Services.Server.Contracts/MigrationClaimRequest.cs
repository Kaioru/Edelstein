using Edelstein.Protocol.Services.Migration.Contracts;

namespace Edelstein.Common.Services.Server.Contracts;

public record MigrationClaimRequest(
    int CharacterID,
    string ServerID,
    long Key
) : IMigrationClaimRequest;
