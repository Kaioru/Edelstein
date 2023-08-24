using AutoMapper;
using Edelstein.Common.Database.Entities;
using Edelstein.Common.Gameplay.Models.Accounts;
using Edelstein.Common.Gameplay.Models.Characters;

namespace Edelstein.Common.Database;

public class GameplayMappingProfile : Profile
{
    public GameplayMappingProfile()
    {
        CreateMap<Account, AccountEntity>().ReverseMap();
        CreateMap<AccountWorld, AccountWorldEntity>().ReverseMap();
        CreateMap<Character, CharacterEntity>().ReverseMap();
    }
}
