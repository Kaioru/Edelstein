﻿using Edelstein.Protocol.Services.Server.Contracts;

namespace Edelstein.Protocol.Services.Server;

public interface IServerService
{
    Task<ServerResponse> RegisterLogin(ServerRegisterRequest<IServerLogin> request);
    Task<ServerResponse> RegisterGame(ServerRegisterRequest<IServerGame> request);
    Task<ServerResponse> RegisterShop(ServerRegisterRequest<IServerShop> request);
    Task<ServerResponse> RegisterTrade(ServerRegisterRequest<IServerTrade> request);

    Task<ServerResponse> Ping(ServerPingRequest request);

    Task<ServerResponse> Deregister(ServerDeregisterRequest request);

    Task<ServerGetOneResponse<IServer>> GetByID(ServerGetByIDRequest request);

    Task<ServerGetOneResponse<IServerGame>> GetGameByWorldAndChannel(ServerGetGameByWorldAndChannelRequest request);
    Task<ServerGetAllResponse<IServerGame>> GetGameByWorld(ServerGetGameByWorldRequest request);

    Task<ServerGetOneResponse<IServerShop>> GetShopByWorld(ServerGetShopByWorldRequest request);
    
    Task<ServerGetOneResponse<IServerTrade>> GetTradeByWorld(ServerGetTradeByWorldRequest request);

    Task<ServerGetAllResponse<IServer>> GetAll();
}
