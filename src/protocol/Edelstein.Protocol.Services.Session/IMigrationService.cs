using Edelstein.Protocol.Services.Session.Contracts;

namespace Edelstein.Protocol.Services.Session;

public interface IMigrationService
{
    Task<IMigrationResponse> Start(IMigrationStartRequest request);
    Task<IMigrationClaimResponse> Claim(IMigrationClaimRequest request);
}
