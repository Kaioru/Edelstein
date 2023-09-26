using System.Collections.Immutable;
using System.Text;
using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Gameplay.Game.Objects.User.Effects;
using Edelstein.Common.Gameplay.Game.Objects.User.Messages;
using Edelstein.Common.Gameplay.Models.Characters.Stats.Modify;
using Edelstein.Common.Gameplay.Models.Inventories.Items;
using Edelstein.Common.Gameplay.Models.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Quests;
using Edelstein.Protocol.Gameplay.Game.Quests.Templates;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests;

public class QuestManager : IQuestManager
{
    private readonly ITemplateManager<IQuestTemplate> _questTemplates;
    private readonly IMobQuestCacheManager _mobQuestCacheManager;
    private readonly ITemplateManager<IItemTemplate> _itemTemplates;
    
    public QuestManager(ITemplateManager<IQuestTemplate> questTemplates, IMobQuestCacheManager mobQuestCacheManager, ITemplateManager<IItemTemplate> itemTemplates)
    {
        _questTemplates = questTemplates;
        _mobQuestCacheManager = mobQuestCacheManager;
        _itemTemplates = itemTemplates;
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
            _ = user.Message(new QuestRecordUpdateMessage(questID, record.Value));
        }
    }
    
    public async Task<QuestResultType> Act(QuestAction action, IQuestTemplate template, IFieldUser user, int? select)
    {
        var actTemplate = action == QuestAction.Start
            ? template.ActStart
            : template.ActEnd;
        var rewardsBase = actTemplate.Items?.Where(i => i.Prob == null).ToImmutableList();
        var rewardsRandom = actTemplate.Items?
            .Where(i => i.Gender is null or 2 || i.Gender == user.Character.Gender)
            .Where(i =>
            {
                var check = false;
                
                if (i.JobFlags > 0)
                {
                    var checks = new List<Tuple<QuestJobFlags, int>>
                    {
                        Tuple.Create(QuestJobFlags.Novice, Job.Novice),
                        Tuple.Create(QuestJobFlags.Swordman, Job.Swordman),
                        Tuple.Create(QuestJobFlags.Magician, Job.Magician),
                        Tuple.Create(QuestJobFlags.Archer, Job.Archer),
                        Tuple.Create(QuestJobFlags.Rogue, Job.Rogue),
                        Tuple.Create(QuestJobFlags.Pirate, Job.Pirate),
                        
                        Tuple.Create(QuestJobFlags.Noblesse, Job.Noblesse),
                        Tuple.Create(QuestJobFlags.Soulfighter, Job.Soulfighter),
                        Tuple.Create(QuestJobFlags.Flamewizard, Job.Flamewizard),
                        Tuple.Create(QuestJobFlags.Windbreaker, Job.Windbreaker),
                        Tuple.Create(QuestJobFlags.Nightwalker, Job.Nightwalker),
                        Tuple.Create(QuestJobFlags.Striker, Job.Striker),
                        
                        Tuple.Create(QuestJobFlags.Legend, Job.Legend),
                        Tuple.Create(QuestJobFlags.Aran, Job.Aran),
                        Tuple.Create(QuestJobFlags.Evan, Job.Evan)
                    };

                    if (checks.Any(c => 
                            i.JobFlags.Value.HasFlag(c.Item1) &&
                            JobConstants.GetJobRace(c.Item2) == JobConstants.GetJobRace(user.Character.Job) &&
                            JobConstants.GetJobType(c.Item2) == JobConstants.GetJobType(user.Character.Job)))
                        check = true;
                }

                if (check) return check;
                
                if (i.JobExFlags > 0)
                {
                    var checks = new List<Tuple<QuestJobExFlags, int>>
                    {
                        Tuple.Create(QuestJobExFlags.Bmage, Job.Bmage),
                        Tuple.Create(QuestJobExFlags.Wildhunter, Job.Wildhunter),
                        Tuple.Create(QuestJobExFlags.Mechanic, Job.Mechanic),
                    };

                    if (checks.Any(c =>
                            i.JobExFlags.Value.HasFlag(c.Item1) &&
                            JobConstants.GetJobRace(c.Item2) == JobConstants.GetJobRace(user.Character.Job) &&
                            JobConstants.GetJobType(c.Item2) == JobConstants.GetJobType(user.Character.Job)))
                        check = true;
                }

                return check;
            })
            .Where(i => i.Prob > 0)
            .ToImmutableList();
        var rewardsSelect = actTemplate.Items?.Where(i => i.Prob == -1).ToDictionary(
            i => i.Order,
            i => i
        );
        var rewardSelect = select != null ? rewardsSelect?.GetValueOrDefault(select.Value) : null;
        var rewardsCheck = new List<Tuple<int, short>>();

        if (rewardsBase != null)
            rewardsCheck.AddRange(rewardsBase
                .Where(r => r.Count > 0)
                .Select(r => Tuple.Create(r.ItemID, (short)r.Count)));
        if (rewardsRandom != null)
            rewardsCheck.AddRange(rewardsRandom
                .GroupBy(r => r.ItemID / 1000000)
                .Select(g => Tuple.Create(g.First().ItemID, (short)g.First().Count)));
        if (rewardSelect != null)
            rewardsCheck.Add(Tuple.Create(rewardSelect.ItemID, (short)rewardSelect.Count));

        if (!user.StageUser.Context.Managers.Inventory.HasSlotFor(user.Character.Inventories, rewardsCheck))
            return QuestResultType.FailedInventory;
        
        var stats = new ModifyStatContext(user.Character);

        if (actTemplate.IncMoney > 0 && stats.Money > int.MaxValue - (actTemplate.IncMoney ?? 0))
            return QuestResultType.FailedMeso;
        if (actTemplate.IncPOP > 0 && stats.POP > short.MaxValue - (actTemplate.IncPOP ?? 0))
            return QuestResultType.FailedUnknown;

        if (actTemplate.IncEXP > 0)
        {
            stats.EXP += actTemplate.IncEXP.Value;
            await user.Message(new IncEXPMessage(actTemplate.IncEXP.Value, true));
        }
        
        if (actTemplate.IncMoney > 0)
        {
            stats.Money += actTemplate.IncMoney.Value;
            await user.Message(new IncMoneyMessage(actTemplate.IncMoney.Value));
        }

        if (actTemplate.IncPOP > 0)
        {
            stats.POP += (short)actTemplate.IncPOP.Value;
            await user.Message(new IncPOPMessage(actTemplate.IncPOP.Value));
        }

        await user.ModifyStats(stats);

        var rewards = new List<IQuestTemplateActItem>();
        
        if (rewardsBase != null)
            rewards.AddRange(rewardsBase);

        if (rewardsRandom != null)
        {
            var random = new Random();
            var value = random.Next(0, rewardsRandom.Sum(r => r.Prob) ?? 0);
            
            foreach (var reward in rewardsRandom)
            {
                value -= reward.Prob ?? 0;

                if (!(value <= 0))
                    continue;

                rewards.Add(reward);
                break;
            }
        }
        
        if (rewardSelect != null)
            rewards.Add(rewardSelect);

        var inventory = new ModifyInventoryGroupContext(user.Character.Inventories, _itemTemplates);
        
        if (rewards.Count > 0)
        {
            var now = DateTime.UtcNow;
            
            foreach (var reward in rewards)
            {
                if (reward.Count > 0)
                {
                    var item = await _itemTemplates.Retrieve(reward.ItemID);
                    var slot = item?.ToItemSlot(reward.Variation ?? ItemVariationOption.None);

                    if (slot is ItemSlotBase slotBase)
                    {
                        if (reward.Period > 0)
                            slotBase.DateExpire = now.AddDays(reward.Period.Value);
                    }
                    
                    if (slot is ItemSlotBundle bundle)
                        bundle.Number = (short)reward.Count;

                    if (slot != null)
                        inventory.Add(slot);
                } else
                    inventory.Remove(reward.ItemID, Math.Abs((short)reward.Count));
            }

            await user.ModifyInventory(inventory);
            await user.Effect(new QuestEffect(rewards
                .Select(r => Tuple.Create(r.ItemID, r.Count))
                .ToImmutableList()));
        }
        
        return QuestResultType.Success;
    }

    public async Task<QuestResultType> Check(QuestAction action, IQuestTemplate template, IFieldUser user)
    {
        var checkTemplate = action == QuestAction.Start
            ? template.CheckStart
            : template.CheckEnd;
        var now = DateTime.UtcNow;
        var record = user.Character.QuestRecords[template.ID]?.Value ?? string.Empty;

        if (checkTemplate.WorldMin != null && user.StageUser.Context.Options.WorldID < checkTemplate.WorldMin)
            return QuestResultType.FailedUnknown;
        if (checkTemplate.WorldMax != null && user.StageUser.Context.Options.WorldID > checkTemplate.WorldMax)
            return QuestResultType.FailedUnknown;
        
        if (checkTemplate.LevelMin != null && user.Character.Level < checkTemplate.LevelMin)
            return QuestResultType.FailedUnknown;
        
        if (checkTemplate.POP != null && user.Character.POP < checkTemplate.POP)
            return QuestResultType.FailedUnknown;
        
        if (checkTemplate.DateStart != null && now < checkTemplate.DateStart)
            return QuestResultType.FailedUnknown;
        if (checkTemplate.DateEnd != null && now > checkTemplate.DateEnd)
            return QuestResultType.FailedUnknown;
        
        if (checkTemplate.Jobs != null && checkTemplate.Jobs.All(j => j != user.Character.Job))
            return QuestResultType.FailedUnknown;
        // TODO: subjob

        if (checkTemplate.CheckItem != null)
        {
            foreach (var item in checkTemplate.CheckItem)
                if (!user.StageUser.Context.Managers.Inventory.HasItem(user.Character.Inventories, item.ItemID, (short)item.Count))
                    return QuestResultType.FailedInventory;
        }
        
        if (action == QuestAction.End && checkTemplate.CheckMob != null)
        {
            if (record.Length != checkTemplate.CheckMob.Count * 3 || !record.All(char.IsDigit))
                return QuestResultType.FailedUnknown;

            foreach (var mob in checkTemplate.CheckMob)
            {
                var count = Convert.ToInt32(record.Substring(3 * mob.Order, 3));
                if (count < mob.Count)
                    return QuestResultType.FailedUnknown;
            }
        }
        
        return QuestResultType.Success;
    }
}
