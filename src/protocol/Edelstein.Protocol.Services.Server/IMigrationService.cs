using Edelstein.Protocol.Services.Server.Contracts;

namespace Edelstein.Protocol.Services.Server;

public interface IMigrationService
{
    Task<MigrationResponse> Start(MigrationStartRequest request);
    Task<MigrationClaimResponse> Claim(MigrationClaimRequest request);
}
