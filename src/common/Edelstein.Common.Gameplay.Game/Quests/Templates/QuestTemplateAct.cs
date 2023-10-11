using System.Collections.Immutable;
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
            .Select(p => (IQuestTemplateActItem)new QuestTemplateActItem(Convert.ToInt32(p.Name), p.ResolveAll()))
            .ToImmutableList();
        Skills = node?.ResolvePath("skill")?.Children
            .Select(p => (IQuestTemplateActSkill)new QuestTemplateActSkill(p.ResolveAll()))
            .ToImmutableList();
        SP = node?.ResolvePath("sp")?.Children
            .Select(p => (IQuestTemplateActSP)new QuestTemplateActSP(p.ResolveAll()))
            .ToImmutableList();
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
