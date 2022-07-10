using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Contracts;
using ProtoBuf.Grpc;

namespace Edelstein.Protocol.Services
{
    [ServiceContract]
    public interface IDispatchService
    {
        [OperationContract] Task<DispatchResponse> Dispatch(DispatchRequest request);
        [OperationContract] Task<DispatchToServersResponse> DispatchToServers(DispatchToServersRequest request);
        [OperationContract] Task<DispatchToCharactersResponse> DispatchToCharacters(DispatchToCharactersRequest request);

        [OperationContract] IAsyncEnumerable<DispatchContract> Subscribe(DispatchSubscription request, CallContext context = default);
    }
}
