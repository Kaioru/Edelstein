using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

public record QuestTemplateCheckMob : IQuestTemplateCheckMob
{
    public int Order { get; }
    
    public int MobID { get; }
    public int Count { get; }

    public QuestTemplateCheckMob(int order, IDataNode node)
    {
        Order = order;
        
        MobID = node.ResolveInt("id") ?? 0;
        Count = node.ResolveInt("count") ?? 0;
    }
}
