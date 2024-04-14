using AutoMapper;
using Edelstein.Common.Services.Server.Entities;
using Edelstein.Protocol.Services.Migration.Contracts;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Services.Session.Contracts;

namespace Edelstein.Common.Services.Server;

public class ServerMappingProfile : Profile
{
    public ServerMappingProfile()
    {
        CreateMap<MigrationEntry, MigrationEntity>().ReverseMap();
        CreateMap<SessionEntry, SessionEntity>().ReverseMap();
        CreateMap<ServerEntry, ServerEntity>().ReverseMap();
        CreateMap<ServerEntryLogin, ServerEntityLogin>().ReverseMap();
        CreateMap<ServerEntryGame, ServerEntityGame>().ReverseMap();
        CreateMap<ServerEntryShop, ServerEntityShop>().ReverseMap();
        CreateMap<ServerEntryTrade, ServerEntityTrade>().ReverseMap();
    }
}
