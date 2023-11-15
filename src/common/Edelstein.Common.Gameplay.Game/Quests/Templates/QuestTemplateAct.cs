using System.Collections.Frozen;
using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

public record QuestTemplateAct : IQuestTemplateAct
{
    public QuestTemplateAct(IDataNode? node)
    {
        IncEXP = node?.ResolveInt("exp");
        IncMoney = node?.ResolveInt("money");
        IncPOP = node?.ResolveInt("pop");
        IncPetTameness = node?.ResolveInt("pop");
        PetSpeed = node?.ResolveInt("petspeed");
        BuffItemID = node?.ResolveInt("buffitemID");
        Info = node?.ResolveString("info");
        NPCAction = node?.ResolveString("npcAct");
        
        NextQuest = node?.ResolveInt("nextQuest");
        
        Items = node?.ResolvePath("item")?.Children
            .Select(p => (IQuestTemplateActItem)new QuestTemplateActItem(Convert.ToInt32(p.Name), p.Cache()))
            .ToFrozenSet();
        Skills = node?.ResolvePath("skill")?.Children
            .Select(p => (IQuestTemplateActSkill)new QuestTemplateActSkill(p.Cache()))
            .ToFrozenSet();
        SP = node?.ResolvePath("sp")?.Children
            .Select(p => (IQuestTemplateActSP)new QuestTemplateActSP(p.Cache()))
            .ToFrozenSet();
    }
    
    public int? IncEXP { get; }
    public int? IncMoney { get; }
    public int? IncPOP { get; }
    public int? IncPetTameness { get; }
    public int? PetSpeed { get; }
    public int? BuffItemID { get; }
    public string? Info { get; }
    public string? NPCAction { get; }

    public int? NextQuest { get; }
    
    public ICollection<IQuestTemplateActItem>? Items { get; }
    public ICollection<IQuestTemplateActSkill>? Skills { get; }
    public ICollection<IQuestTemplateActSP>? SP { get; }
}
