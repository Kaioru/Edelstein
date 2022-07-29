using Edelstein.Protocol.Services.Session.Contracts;
using Edelstein.Protocol.Services.Session.Types;

namespace Edelstein.Protocol.Services.Session;

public interface IServerService
{
    Task<IServerRegisterResponse> RegisterLogin(IServerRegisterRequest<IServerLogin> request);
    Task<IServerRegisterResponse> RegisterGame(IServerRegisterRequest<IServerGame> request);

    Task<IServerPingResponse> Ping(IServerPingRequest request);

    Task<IServerDeregisterResponse> Deregister(IServerDeregisterRequest request);

    Task<IServerGetOneResponse<IServer>> GetByID(IServerGetByIDRequest request);

    Task<IServerGetOneResponse<IServerGame>> GetGameByWorldAndChannel(IServerGetGameByWorldAndChannel request);
    Task<IServerGetAllResponse<IServerGame>> GetGameByWorld(IServerGetGameByWorld request);

    Task<IServerGetAllResponse<IServer>> GetAll();
}
