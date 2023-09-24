namespace Edelstein.Protocol.Gameplay.Game.Quests.Templates;

public interface IQuestTemplateAct
{
    int? IncEXP { get; }
    int? IncMoney { get; }
    int? IncPOP { get; }
    int? IncPetTameness { get; }
    int? PetSpeed { get; }
    int? BuffItemID { get; }
    
    string? Info { get; }
    
    string? NPCAction { get; }
    
    int? NextQuest { get; }
    
    ICollection<IQuestTemplateActItem>? Items { get; }
    ICollection<IQuestTemplateActSkill>? Skills { get; }
}
