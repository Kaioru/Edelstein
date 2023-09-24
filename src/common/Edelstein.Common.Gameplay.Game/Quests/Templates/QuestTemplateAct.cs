using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

public record QuestTemplateAct : IQuestTemplateAct
{
    public QuestTemplateAct(IDataProperty? property)
    {
        IncEXP = property?.Resolve<int>("exp");
        IncMoney = property?.Resolve<int>("money");
        
        NextQuest = property?.Resolve<int>("nextQuest");
    }
    
    public int? IncEXP { get; }
    public int? IncMoney { get; }
    
    public int? NextQuest { get; }
}
