using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

public record QuestTemplateCheckMob : IQuestTemplateCheckMob
{
    public int Order { get; }
    
    public int MobID { get; }
    public int Count { get; }

    public QuestTemplateCheckMob(int order, IDataProperty property)
    {
        Order = order;
        
        MobID = property.Resolve<int>("id") ?? 0;
        Count = property.Resolve<int>("count") ?? 0;
    }
}
