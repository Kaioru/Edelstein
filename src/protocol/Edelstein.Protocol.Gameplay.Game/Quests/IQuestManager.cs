using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Quests;

public interface IQuestManager
{
    Task<QuestResultType> Act(QuestAction action, IQuestTemplate template, IFieldUser user);
    Task<QuestResultType> Check(QuestAction action, IQuestTemplate template, IFieldUser user);
}
