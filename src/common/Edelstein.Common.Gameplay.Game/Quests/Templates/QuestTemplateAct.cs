using System.Collections.Immutable;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

public record QuestTemplateAct : IQuestTemplateAct
{
    public QuestTemplateAct(IDataProperty? property)
    {
        IncEXP = property?.Resolve<int>("exp");
        IncMoney = property?.Resolve<int>("money");
        IncPOP = property?.Resolve<int>("pop");
        IncPetTameness = property?.Resolve<int>("pop");
        PetSpeed = property?.Resolve<int>("petspeed");
        BuffItemID = property?.Resolve<int>("buffitemID");
        Info = property?.ResolveOrDefault<string>("info");
        NPCAction = property?.ResolveOrDefault<string>("npcAct");
        
        NextQuest = property?.Resolve<int>("nextQuest");
        
        Items = property?.Resolve("item")?.Children
            .Select(p => (IQuestTemplateActItem)new QuestTemplateActItem(Convert.ToInt32(p.Name), p.ResolveAll()))
            .ToImmutableList();
        Skills = property?.Resolve("skill")?.Children
            .Select(p => (IQuestTemplateActSkill)new QuestTemplateActSkill(p.ResolveAll()))
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
}
