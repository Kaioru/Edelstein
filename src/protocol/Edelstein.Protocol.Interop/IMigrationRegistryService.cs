using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Interop.Contracts;

namespace Edelstein.Protocol.Interop
{
    [ServiceContract]
    public interface IMigrationRegistryService
    {
        [OperationContract] Task<MigrationRegistryResponse> Register(RegisterMigrationRequest request);
        [OperationContract] Task<MigrationRegistryResponse> Deregister(DeregisterMigrationRequest request);
        [OperationContract] Task<ClaimMigrationResponse> Claim(ClaimMigrationRequest request);
    }
}
