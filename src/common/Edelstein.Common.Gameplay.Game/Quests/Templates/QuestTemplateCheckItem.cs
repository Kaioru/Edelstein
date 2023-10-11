using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

public record QuestTemplateCheckItem : IQuestTemplateCheckItem
{
    public int ItemID { get; }
    public int Count { get; }
    
    public QuestTemplateCheckItem(IDataNode node)
    {
        ItemID = node.ResolveInt("id") ?? 0;
        Count = node.ResolveInt("count") ?? 0;
    }
}
