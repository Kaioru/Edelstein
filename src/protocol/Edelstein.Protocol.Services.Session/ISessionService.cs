using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Session.Contracts;
using ProtoBuf.Grpc;

namespace Edelstein.Protocol.Services.Session;

[ServiceContract]
public interface ISessionService
{
    [OperationContract] 
    Task<SessionServiceResponse> Start(SessionServiceStartRequest request, CallContext context = default);
    
    [OperationContract] 
    Task<SessionServiceResponse> End(SessionServiceEndRequest request, CallContext context = default);

    [OperationContract] 
    Task<SessionServiceResponse> UpdateServer(SessionServiceUpdateServerRequest request, CallContext context = default);
    
    [OperationContract] 
    Task<SessionServiceResponse> UpdateCharacter(SessionServiceUpdateCharacterRequest request, CallContext context = default);

    [OperationContract] 
    Task<SessionServiceGetOneResponse> GetByActiveAccount(SessionServiceGetByActiveAccountRequest request, CallContext context = default);
    
    [OperationContract] 
    Task<SessionServiceGetOneResponse> GetByActiveCharacter(SessionServiceGetByActiveCharacterRequest request, CallContext context = default);
}
