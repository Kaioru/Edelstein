using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Services.Server.Contracts.Requests;
using Edelstein.Protocol.Services.Server.Contracts.Responses;
using ProtoBuf.Grpc;

namespace Edelstein.Protocol.Services.Server;

[ServiceContract]
public interface IServerService
{
    [OperationContract] Task<ServerRegisterResponse> RegisterLogin(ServerRegisterRequest<ServerEntryLogin> request, CallContext context = default);
    [OperationContract] Task<ServerRegisterResponse> RegisterGame(ServerRegisterRequest<ServerEntryGame> request, CallContext context = default);
    [OperationContract] Task<ServerRegisterResponse> RegisterShop(ServerRegisterRequest<ServerEntryShop> request, CallContext context = default);
    [OperationContract] Task<ServerRegisterResponse> RegisterTrade(ServerRegisterRequest<ServerEntryTrade> request, CallContext context = default);

    [OperationContract] Task<ServerResponse> Update(ServerUpdateRequest request, CallContext context = default);
    
    [OperationContract] Task<ServerResponse> Deregister(ServerDeregisterRequest request, CallContext context = default);
    
    [OperationContract] Task<ServerGetOneResponse<ServerEntry>> GetByID(ServerGetByIDRequest request, CallContext context = default);
    
    [OperationContract] Task<ServerGetOneResponse<ServerEntryGame>> GetGameByWorldAndChannel(ServerGetGameByWorldAndChannelRequest request, CallContext context = default);
    [OperationContract] Task<ServerGetAllResponse<ServerEntryGame>> GetGameByWorld(ServerGetGameByWorldRequest request, CallContext context = default);
    
    [OperationContract] Task<ServerGetOneResponse<ServerEntryShop>> GetShopByWorld(ServerGetShopByWorldRequest request, CallContext context = default);
    
    [OperationContract] Task<ServerGetOneResponse<ServerEntryTrade>> GetTradeByWorld(ServerGetTradeByWorldRequest request, CallContext context = default);
    
    [OperationContract] Task<ServerGetAllResponse<ServerEntry>> GetAll(CallContext context = default);
}
