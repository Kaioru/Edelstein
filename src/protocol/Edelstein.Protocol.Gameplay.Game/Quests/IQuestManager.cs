using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Quests;

public interface IQuestManager
{
    Task UpdateMobKill(IFieldUser user, int mobID, int inc = 1);

    Task<QuestResultType> Accept(IFieldUser user, int questID);
    Task<QuestResultType> Complete(IFieldUser user, int questID, int? select = null);
    Task<QuestResultType> Resign(IFieldUser user, int questID);
}
