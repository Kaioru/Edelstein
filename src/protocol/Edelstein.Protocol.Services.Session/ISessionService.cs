using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Session.Contracts.Requests;
using Edelstein.Protocol.Services.Session.Contracts.Responses;
using ProtoBuf.Grpc;

namespace Edelstein.Protocol.Services.Session;

[ServiceContract]
public interface ISessionService
{
    [OperationContract] Task<SessionResponse> Start(SessionStartRequest request, CallContext context = default);
    [OperationContract] Task<SessionResponse> End(SessionEndRequest request, CallContext context = default);
    
    [OperationContract] Task<SessionResponse> UpdateServer(SessionUpdateServerRequest request, CallContext context = default);
    [OperationContract] Task<SessionResponse> UpdateCharacter(SessionUpdateCharacterRequest request, CallContext context = default);
    
    [OperationContract] Task<SessionGetOneResponse> GetByActiveAccount(SessionGetByActiveAccountRequest request, CallContext context = default);
    [OperationContract] Task<SessionGetOneResponse> GetByActiveCharacter(SessionGetByActiveCharacterRequest request, CallContext context = default);
}
