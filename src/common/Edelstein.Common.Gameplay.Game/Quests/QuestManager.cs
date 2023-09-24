using Edelstein.Common.Gameplay.Game.Objects.User.Messages;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Quests;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests;

public class QuestManager : IQuestManager
{
    public async Task<QuestResultType> Act(QuestAction action, IQuestTemplate template, IFieldUser user)
    {
        var actTemplate = action == QuestAction.Start
            ? template.ActStart
            : template.ActEnd;

        if (actTemplate.IncEXP > 0)
        {
            await user.ModifyStats(s => s.EXP += actTemplate.IncEXP.Value);
            await user.Message(new IncEXPMessage(actTemplate.IncEXP.Value, true));
        }
        
        if (actTemplate.IncMoney > 0)
        {
            await user.ModifyStats(s => s.Money += actTemplate.IncMoney.Value);
            await user.Message(new IncMoneyMessage(actTemplate.IncMoney.Value));
        }
        
        return QuestResultType.Success;
    }

    public async Task<QuestResultType> Check(QuestAction action, IQuestTemplate template, IFieldUser user)
    {
        var checkTemplate = action == QuestAction.Start
            ? template.CheckStart
            : template.CheckEnd;
        return QuestResultType.Success;
    }
}
