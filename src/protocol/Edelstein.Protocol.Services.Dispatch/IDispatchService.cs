using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Dispatch.Contracts;
using ProtoBuf.Grpc;

namespace Edelstein.Protocol.Services.Dispatch;

[ServiceContract]
public interface IDispatchService
{
    [OperationContract]
    Task<DispatchServiceResponse> Send(DispatchServiceSendRequest request, CallContext context = default);
    
    [OperationContract]
    IAsyncEnumerable<DispatchServiceSubscribeResponse> Subscribe(DispatchServiceSubscribeRequest request, CallContext context = default);
}
