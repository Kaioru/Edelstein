using Edelstein.Protocol.Services.Migration.Contracts;

namespace Edelstein.Common.Services.Server.Contracts;

public record MigrationResponse(MigrationResult Result) : IMigrationResponse;
