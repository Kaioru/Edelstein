using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

public record QuestTemplateCheckItem : IQuestTemplateCheckItem
{
    public int ItemID { get; }
    public int Count { get; }
    
    public QuestTemplateCheckItem(IDataProperty property)
    {
        ItemID = property.Resolve<int>("id") ?? 0;
        Count = property.Resolve<int>("count") ?? 0;
    }
}
