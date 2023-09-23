namespace Edelstein.Protocol.Gameplay.Models.Characters.Quests.Templates;

public interface IQuestTemplateCheck
{
    int? WorldMin { get; }
    int? WorldMax { get; }
    
    int? TamingMobLevelMin { get; }
    int? TamingMobLevelMax { get; }
    int? PetTamenessMin { get; }
    int? PetTamenessMax { get; }
    
    int? LevelMin { get; }
    int? LevelMax { get; }
    
    int? POP { get; }
    
    DateTime? DateStart { get; }
    DateTime? DateEnd { get; }
    
    int? Job { get; }
    int? SubJobFlags { get; }
}
