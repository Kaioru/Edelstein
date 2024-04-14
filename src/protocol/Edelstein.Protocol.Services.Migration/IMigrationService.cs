using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Migration.Contracts.Requests;
using Edelstein.Protocol.Services.Migration.Contracts.Responses;
using ProtoBuf.Grpc;

namespace Edelstein.Protocol.Services.Migration;

[ServiceContract]
public interface IMigrationService
{
    [OperationContract] Task<MigrationResponse> Start(MigrationStartRequest request, CallContext context = default);
    [OperationContract] Task<MigrationClaimResponse> Claim(MigrationClaimRequest request, CallContext context = default);
}
