using Edelstein.Protocol.Services.Migration.Contracts;

namespace Edelstein.Common.Services.Server.Contracts;

public record MigrationClaimRequest(
    int ID,
    string ServerID,
    long Key
) : IMigrationClaimRequest;
