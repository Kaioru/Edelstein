using AutoMapper;
using Edelstein.Common.Database.Entities;
using Edelstein.Common.Database.Entities.Services.Auth;
using Edelstein.Common.Database.Entities.Services.Migration;
using Edelstein.Common.Database.Entities.Services.Server;
using Edelstein.Common.Database.Entities.Services.Session;
using Edelstein.Common.Services.Auth;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Services.Migration.Contracts;
using Edelstein.Protocol.Services.Server.Contracts;
using Edelstein.Protocol.Services.Session.Contracts;

namespace Edelstein.Common.Database;

public class GameDbMappingProfile : Profile
{
    public GameDbMappingProfile()
    {
        CreateMap<DbAccount, Account>().ReverseMap();
        CreateMap<DbAccountWorldData, AccountWorldData>().ReverseMap();
        CreateMap<DbCharacter, Character>().ReverseMap();
        
        CreateMap<DbIdentity, Identity>().ReverseMap();
        
        CreateMap<DbServerInfo, ServerInfo>().ReverseMap();
        CreateMap<DbServerInfoLogin, ServerInfoLogin>().ReverseMap();
        CreateMap<DbServerInfoGame, ServerInfoGame>().ReverseMap();
        
        CreateMap<DbMigrationInfo, MigrationInfo>().ReverseMap();
        CreateMap<DbSessionInfo, SessionInfo>().ReverseMap();
    }
}
