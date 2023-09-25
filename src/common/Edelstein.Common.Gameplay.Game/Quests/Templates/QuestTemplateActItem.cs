using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

public record QuestTemplateActItem : IQuestTemplateActItem
{
    public QuestTemplateActItem(int order, IDataProperty property)
    {
        Order = order;

        ItemID = property.Resolve<int>("id") ?? 0;
        Count = property.Resolve<int>("count") ?? 0;

        IsNamed = property.Resolve<int>("name") > 0;
        Period = property.Resolve<int>("period");
        JobFlags = (QuestJobFlags?)property.Resolve<int>("job");
        JobExFlags = (QuestJobExFlags?)property.Resolve<int>("jobEx");
        Gender = property.Resolve<int>("gender");
        Prob = property.Resolve<int>("prop");
        Variation = (ItemVariationOption?)property.Resolve<int>("var");
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
