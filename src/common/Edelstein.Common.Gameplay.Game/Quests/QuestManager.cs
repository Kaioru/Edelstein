﻿using System.Collections.Immutable;
using System.Text;
using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Gameplay.Game.Objects.User.Messages;
using Edelstein.Common.Gameplay.Models.Inventories.Items;
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
            _ = user.Message(new QuestRecordAcceptMessage(questID, record.Value));
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

                    if (checks.Any(c => i.JobFlags.Value.HasFlag(c.Item1) &&
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
        if (rewardsRandom != null) // TODO: job check
            rewardsCheck.AddRange(rewardsRandom
                .GroupBy(r => r.ItemID / 1000000)
                .Select(g => Tuple.Create(g.First().ItemID, (short)g.First().Count)));
        if (rewardSelect != null)
            rewardsCheck.Add(Tuple.Create(rewardSelect.ItemID, (short)rewardSelect.Count));

        if (!user.StageUser.Context.Managers.Inventory.HasSlotFor(user.Character.Inventories, rewardsCheck))
            return QuestResultType.FailedInventory;
        
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

        if (actTemplate.IncPOP > 0)
        {
            await user.ModifyStats(s => s.POP += (short)actTemplate.IncPOP.Value);
            await user.Message(new IncPOPMessage(actTemplate.IncPOP.Value));
        }

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
                        await user.ModifyInventory(i => i.Add(slot));
                } else
                    await user.ModifyInventory(i => i.Remove(reward.ItemID, Math.Abs((short)reward.Count)));
            }
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
