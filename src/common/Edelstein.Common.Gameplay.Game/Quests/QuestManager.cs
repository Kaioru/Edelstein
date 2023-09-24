using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Quests;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests;

public class QuestManager : IQuestManager
{
    public Task<QuestResultType> Act(QuestAction action, IQuestTemplate template, int NPCTemplateID, IFieldUser user)
        => Task.FromResult(QuestResultType.Success);

    public Task<QuestResultType> Check(QuestAction action, IQuestTemplate template, int NPCTemplateID, IFieldUser user) 
        => Task.FromResult(QuestResultType.Success);
}
