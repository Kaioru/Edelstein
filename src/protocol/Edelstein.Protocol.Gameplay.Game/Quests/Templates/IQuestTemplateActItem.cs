using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Quests.Templates;

public interface IQuestTemplateActItem
{
    int Order { get; }
    
    int ItemID { get; }
    int Count { get; }
    
    bool? IsNamed { get; }
    int? Period { get; }
    QuestJobFlags? JobFlags { get; }
    QuestJobExFlags? JobExFlags { get; }
    int? Gender { get; }
    int? Prob { get; }
    ItemVariationOption? Variation { get; }
}
