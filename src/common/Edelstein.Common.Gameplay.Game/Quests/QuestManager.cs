using System.Text;
using Edelstein.Common.Gameplay.Game.Objects.User.Messages;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Quests;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests;

public class QuestManager : IQuestManager
{
    private readonly ITemplateManager<IQuestTemplate> _questTemplates;
    private readonly IMobQuestCacheManager _mobQuestCacheManager;
    
    public QuestManager(ITemplateManager<IQuestTemplate> questTemplates, IMobQuestCacheManager mobQuestCacheManager)
    {
        _questTemplates = questTemplates;
        _mobQuestCacheManager = mobQuestCacheManager;
    }
    
    public async Task UpdateMobKill(IFieldUser user, int mobID, int inc)
    {
        var questRecords = user.Character.QuestRecords;
        var mobQuestCache = await _mobQuestCacheManager.Retrieve(mobID);
        if (mobQuestCache == null || mobQuestCache.Quests.Count == 0) return;
        
        foreach (var questID in mobQuestCache.Quests)
        {
            if (!questRecords.Records.TryGetValue(questID, out var record)) continue;
            
            var quest = await _questTemplates.Retrieve(questID);
            if (quest?.CheckEnd.CheckMob == null) continue;
            
            if (record.Value.Length != quest.CheckEnd.CheckMob.Count * 3 || !record.Value.All(char.IsDigit))
            {
                record.Value = string.Empty;
                for (var i = 0; i < quest.CheckEnd.CheckMob.Count; i++)
                    record.Value += "000";
            }

            var builder = new StringBuilder(record.Value, quest.CheckEnd.CheckMob.Count * 3);
            var checkMob = quest.CheckEnd.CheckMob.FirstOrDefault(m => m.MobID == mobID);
            if (checkMob == null) continue;
            
            var count = Convert.ToInt32(builder.ToString(3 * checkMob.Order, 3));

            if (count >= checkMob.Count) continue;

            count += inc;
            count = Math.Min(count, checkMob.Count);
            count = Math.Max(count, 0);

            builder.Remove(3 * checkMob.Order, 3);
            builder.Insert(3 * checkMob.Order, count.ToString("000"));
            
            record.Value = builder.ToString();
            _ = user.Message(new QuestRecordAcceptMessage(questID, record.Value));
        }
    }
    
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
