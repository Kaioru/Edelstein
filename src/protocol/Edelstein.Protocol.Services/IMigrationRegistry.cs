using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Contracts;

namespace Edelstein.Protocol.Services
{
    [ServiceContract]
    public interface IMigrationRegistry
    {
        [OperationContract] Task<RegisterMigrationResponse> Register(RegisterMigrationRequest request);
        [OperationContract] Task<DeregisterMigrationResponse> Deregister(DeregisterMigrationRequest request);
        [OperationContract] Task<ClaimMigrationResponse> Claim(ClaimMigrationRequest request);
    }
}
