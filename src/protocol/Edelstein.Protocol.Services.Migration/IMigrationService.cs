using Edelstein.Protocol.Services.Migration.Contracts;

namespace Edelstein.Protocol.Services.Migration;

public interface IMigrationService
{
    Task<MigrationResponse> Start(MigrationStartRequest request);
    Task<MigrationClaimResponse> Claim(MigrationClaimRequest request);
}
