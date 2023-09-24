namespace Edelstein.Protocol.Gameplay.Game.Quests.Templates;

public interface IQuestTemplateCheck
{
    string? ScriptStart { get; }
    string? ScriptEnd { get; }
    
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
    
    ICollection<int>? Jobs { get; }
    int? SubJobFlags { get; }
}
