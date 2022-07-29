using Edelstein.Protocol.Services.Session.Contracts;
using Edelstein.Protocol.Services.Session.Types;

namespace Edelstein.Protocol.Services.Session;

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
