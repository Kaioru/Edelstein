using Edelstein.Protocol.Services.Migration.Contracts;

namespace Edelstein.Common.Services.Server.Contracts;

public record MigrationClaimRequest(string Key, int ID) : IMigrationClaimRequest;
