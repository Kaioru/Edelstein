using AutoMapper;
using Edelstein.Application.Server.Bindings;
using Edelstein.Protocol.Services.Server.Contracts;

namespace Edelstein.Application.Server;

public class ProgramMappingProfile : Profile
{
    public ProgramMappingProfile()
    {
        CreateMap<LoginStageSystemConfig, ServerInfoLogin>().ReverseMap();
        CreateMap<GameStageSystemConfig, ServerInfoGame>().ReverseMap();
    }
}
