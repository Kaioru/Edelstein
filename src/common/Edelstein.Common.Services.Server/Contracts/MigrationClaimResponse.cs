using Edelstein.Protocol.Services.Migration.Contracts;
using Edelstein.Protocol.Services.Migration.Types;

namespace Edelstein.Common.Services.Server.Contracts;

public record MigrationClaimResponse(
    MigrationResult Result,
    IMigration? Migration = null
) : IMigrationClaimResponse;
