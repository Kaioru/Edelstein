using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Services.Server.Types;

namespace Edelstein.Protocol.Services.Server;

public interface IServerService
{
    Task<IServerResponse> RegisterLogin(IServerRegisterRequest<IServerLogin> request);
    Task<IServerResponse> RegisterGame(IServerRegisterRequest<IServerGame> request);

    Task<IServerResponse> Ping(IServerPingRequest request);

    Task<IServerResponse> Deregister(IServerDeregisterRequest request);

    Task<IServerGetOneResponse<IServer>> GetByID(IServerGetByIDRequest request);

    Task<IServerGetOneResponse<IServerGame>> GetGameByWorldAndChannel(IServerGetGameByWorldAndChannel request);
    Task<IServerGetAllResponse<IServerGame>> GetGameByWorld(IServerGetGameByWorld request);

    Task<IServerGetAllResponse<IServer>> GetAll();
}
