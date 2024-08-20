using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Server.Contracts;
using ProtoBuf.Grpc;

namespace Edelstein.Protocol.Services.Server;

[ServiceContract]
public interface IServerService
{
    [OperationContract]
    Task<ServerServiceRegisterResponse> RegisterLogin(ServerServiceRegisterRequest<ServerInfoLogin> request, CallContext context = default);
    
    [OperationContract]
    Task<ServerServiceResponse> Ping(ServerServicePingRequest request, CallContext context = default);
    
    [OperationContract]
    Task<ServerServiceResponse> Deregister(ServerServiceDeregisterRequest request, CallContext context = default);
    
    [OperationContract]
    Task<ServerServiceGetOneResponse<ServerInfo>> GetByID(ServerServiceGetByIDRequest request, CallContext context = default);
    
    [OperationContract]
    Task<ServerServiceGetAllResponse<ServerInfo>> GetAll(CallContext context = default);
}
