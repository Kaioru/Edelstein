using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

public record QuestTemplateActItem : IQuestTemplateActItem
{
    public QuestTemplateActItem(int order, IDataNode node)
    {
        Order = order;

        ItemID = node.ResolveInt("id") ?? 0;
        Count = node.ResolveInt("count") ?? 0;

        IsNamed = node.ResolveInt("name") > 0;
        Period = node.ResolveInt("period");
        JobFlags = (QuestJobFlags?)node.ResolveInt("job");
        JobExFlags = (QuestJobExFlags?)node.ResolveInt("jobEx");
        Gender = node.ResolveInt("gender");
        Prob = node.ResolveInt("prop");
        Variation = (ItemVariationOption?)node.ResolveInt("var");
    }
    
    public int Order { get; }
    
    public int ItemID { get; }
    public int Count { get; }
    
    public bool? IsNamed { get; }
    public int? Period { get; }
    public QuestJobFlags? JobFlags { get; }
    public QuestJobExFlags? JobExFlags { get; }
    public int? Gender { get; }
    public int? Prob { get; }
    public ItemVariationOption? Variation { get; }
}
