using Edelstein.Protocol.Services.Migration.Contracts;

namespace Edelstein.Protocol.Services.Migration;

public interface IMigrationService
{
    Task<IMigrationResponse> Start(IMigrationStartRequest request);
    Task<IMigrationClaimResponse> Claim(IMigrationClaimRequest request);
}
