using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Quests;

public interface IQuestManager
{
    Task UpdateMobKill(IFieldUser user, int mobID, int inc = 1);

    Task Accept(IFieldUser user, int questID);
    Task Complete(IFieldUser user, int questID);
    Task Resign(IFieldUser user, int questID);
    
    Task<QuestResultType> Act(QuestAction action, IQuestTemplate template, IFieldUser user, int? select);
    Task<QuestResultType> Check(QuestAction action, IQuestTemplate template, IFieldUser user);
}
