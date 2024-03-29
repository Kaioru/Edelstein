﻿using AutoMapper;
using Edelstein.Common.Services.Server.Entities;
using Edelstein.Protocol.Services.Server.Contracts;

namespace Edelstein.Common.Services.Server;

public class ServerMappingProfile : Profile
{
    public ServerMappingProfile()
    {
        CreateMap<Migration, MigrationEntity>().ReverseMap();
        CreateMap<Session, SessionEntity>().ReverseMap();
        CreateMap<Protocol.Services.Server.Contracts.Server, ServerEntity>().ReverseMap();
        CreateMap<ServerLogin, ServerLoginEntity>().ReverseMap();
        CreateMap<ServerGame, ServerGameEntity>().ReverseMap();
        CreateMap<ServerShop, ServerShopEntity>().ReverseMap();
        CreateMap<ServerTrade, ServerTradeEntity>().ReverseMap();
    }
}
