using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Migration.Contracts;
using ProtoBuf.Grpc;

namespace Edelstein.Protocol.Services.Migration;

[ServiceContract]
public interface IMigrationService
{
    [OperationContract]
    Task<MigrationServiceResponse> Start(MigrationServiceStartRequest request, CallContext context = default);
    
    [OperationContract]
    Task<MigrationServiceClaimResponse> Claim(MigrationServiceClaimRequest request, CallContext context = default);
}
