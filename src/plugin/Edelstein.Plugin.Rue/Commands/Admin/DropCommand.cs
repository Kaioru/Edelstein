using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Game.Objects.Mob.Rewards;
using Edelstein.Common.Gameplay.Game.Rewards;
using Edelstein.Protocol.Gameplay.Game.Rewards;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Rewards;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using PowerArgs;

namespace Edelstein.Plugin.Rue.Commands.Admin;

/// <summary>
/// Command for managing runtime synthetic mob drop tables.
/// Supports add/remove/copy/swap operations and drop simulation testing.
/// </summary>
public sealed class DropCommand : AbstractCommand<DropCommandArgs>
{
    private readonly IMobRewardPoolManager _rewardPool;
    private int _nextRewardId = int.MaxValue;

    public override string Name => "Drop";
    public override string Description => "Manage synthetic drops for mobs at runtime";

    public DropCommand(IMobRewardPoolManager rewardPool)
    {
        _rewardPool = rewardPool;
        Aliases.Add("Drops");
    }

    private int GetNextRewardId() => Interlocked.Decrement(ref _nextRewardId);

    private MobReward CloneReward(IMobReward source) => new(GetNextRewardId(), source.Proc)
    {
        ItemID = source.ItemID,
        Money = source.Money,
        NumberMin = source.NumberMin,
        NumberMax = source.NumberMax,
        ReqQuest = source.ReqQuest,
        ReqLevelMin = source.ReqLevelMin,
        ReqLevelMax = source.ReqLevelMax,
        ReqMobLevelMin = source.ReqMobLevelMin,
        ReqMobLevelMax = source.ReqMobLevelMax,
        DateStart = source.DateStart,
        DateEnd = source.DateEnd
    };

    private async Task<IRewardPool<IMobReward>> GetOrCreatePoolAsync(int mobId)
    {
        var pool = await _rewardPool.Retrieve(mobId);
        if (pool != null) return pool;

        pool = new RewardPool<IMobReward>(mobId);
        await _rewardPool.Insert(pool);
        return pool;
    }

    protected override async Task Execute(IFieldUser user, DropCommandArgs args)
    {
        var action = args.Action?.ToLowerInvariant() ?? "help";

        switch (action)
        {
            case "add":
                await ExecuteAdd(user, args);
                break;
            case "addmoney":
                await ExecuteAddMoney(user, args);
                break;
            case "global":
                await ExecuteGlobal(user, args);
                break;
            case "list":
                await ExecuteList(user, args);
                break;
            case "clear":
                await ExecuteClear(user, args);
                break;
            case "clearglobal":
                await ExecuteClearGlobal(user);
                break;
            case "random":
                await ExecuteRandom(user, args);
                break;
            case "swap":
                await ExecuteSwap(user, args);
                break;
            case "copy":
                await ExecuteCopy(user, args);
                break;
            case "remove":
                await ExecuteRemove(user, args);
                break;
            case "test":
                await ExecuteTest(user, args);
                break;
            default:
                await ShowHelp(user);
                break;
        }
    }

    private async Task ExecuteAdd(IFieldUser user, DropCommandArgs args)
    {
        if (args.MobId is not { } mobId || args.ItemId is not { } itemId)
        {
            await user.Message("Usage: /drop add <mobId> <itemId> [rate] [min] [max]");
            return;
        }

        var pool = await GetOrCreatePoolAsync(mobId);
        var reward = new MobReward(GetNextRewardId(), args.Rate)
        {
            ItemID = itemId,
            NumberMin = args.Min,
            NumberMax = args.Max
        };
        await pool.Insert(reward);

        await user.Message($"Added item drop {itemId} to mob {mobId} (rate: {args.Rate:P0}, qty: {args.Min ?? 1}-{args.Max ?? 1})");
    }

    private async Task ExecuteAddMoney(IFieldUser user, DropCommandArgs args)
    {
        if (args.MobId is not { } mobId || args.ItemId is not { } amount)
        {
            await user.Message("Usage: /drop addmoney <mobId> <amount> [rate]");
            return;
        }

        var pool = await GetOrCreatePoolAsync(mobId);
        var reward = new MobReward(GetNextRewardId(), args.Rate)
        {
            Money = amount
        };
        await pool.Insert(reward);

        await user.Message($"Added money drop {amount} to mob {mobId} (rate: {args.Rate:P0})");
    }

    /// <summary>
    /// Adds a global drop that applies to all mobs.
    /// Parameter mapping differs from other commands:
    ///   /drop global [itemId] [rate%] [min] [max]
    ///   - args.MobId  -> itemId (position 1)
    ///   - args.ItemId -> rate as 0-100 percent (position 2)
    ///   - args.Rate   -> min quantity (position 3)
    ///   - args.Min    -> max quantity (position 4)
    /// </summary>
    private async Task ExecuteGlobal(IFieldUser user, DropCommandArgs args)
    {
        if (args.MobId is not { } itemId)
        {
            await user.Message("Usage: /drop global <itemId> [rate%] [min] [max]");
            await user.Message("  rate% is 0-100 (default 100)");
            return;
        }

        var ratePercent = args.ItemId ?? 100;
        var rate = Math.Clamp(ratePercent / 100.0, 0.0, 1.0);
        var min = (int?)args.Rate != 0 ? (int)args.Rate : (int?)null;
        var max = args.Min;

        var reward = new MobReward(GetNextRewardId(), rate)
        {
            ItemID = itemId,
            NumberMin = min,
            NumberMax = max
        };
        await _rewardPool.Global.Insert(reward);

        await user.Message($"Added global item drop {itemId} (rate: {rate:P0}, qty: {min ?? 1}-{max ?? 1})");
    }

    private async Task ExecuteList(IFieldUser user, DropCommandArgs args)
    {
        var output = "#e#bDrop Table#n\\r\\n";

        if (args.MobId is { } mobId)
        {
            if (mobId == -1)
            {
                // -1 = show global drops
                output += "\\r\\n#eGlobal Drops:#n\\r\\n";
                var globalRewards = await _rewardPool.Global.RetrieveAll();
                if (globalRewards.Count == 0)
                    output += "  (none)\\r\\n";
                else
                    foreach (var reward in globalRewards)
                        output += FormatReward(reward);
            }
            else
            {
                var pool = await _rewardPool.Retrieve(mobId);
                if (pool == null)
                {
                    await user.Message($"No drops for mob {mobId}");
                    return;
                }

                output += $"\\r\\n#eMob {mobId}:#n\\r\\n";
                foreach (var reward in await pool.RetrieveAll())
                    output += FormatReward(reward);
            }
        }
        else
        {
            if (user.Field == null)
            {
                await user.Message("You must be in a field, or specify a mobId");
                return;
            }

            var mobPool = user.Field.GetPool(FieldObjectType.Mob);
            var mobs = (mobPool?.Objects ?? ImmutableArray<IFieldObject>.Empty)
                .OfType<IFieldMob>()
                .ToList();

            if (mobs.Count == 0)
            {
                await user.Message("No mobs on this field");
                return;
            }

            var uniqueMobIds = mobs.Select(m => m.Template.ID).Distinct().OrderBy(x => x).ToList();

            foreach (var fieldMobId in uniqueMobIds)
            {
                var pool = await _rewardPool.Retrieve(fieldMobId);
                var rewards = pool != null ? await pool.RetrieveAll() : Array.Empty<IMobReward>();

                output += $"\\r\\n#eMob {fieldMobId}#n ({rewards.Count} drops):\\r\\n";
                if (rewards.Count == 0)
                    output += "  (none)\\r\\n";
                else
                    foreach (var reward in rewards)
                        output += FormatReward(reward);
            }

            var globalRewards = await _rewardPool.Global.RetrieveAll();
            if (globalRewards.Count > 0)
            {
                output += $"\\r\\n#eGlobal#n ({globalRewards.Count} drops):\\r\\n";
                foreach (var reward in globalRewards)
                    output += FormatReward(reward);
            }
        }

        await user.Prompt(s => s.Say(output), default);
    }

    private static string FormatReward(IMobReward reward)
    {
        var line = $"  #{reward.ID}: ";

        if (reward.ItemID is { } itemId)
            line += $"Item {itemId} @ {reward.Proc:P0} x{reward.NumberMin ?? 1}-{reward.NumberMax ?? 1}";
        else if (reward.Money is { } money)
            line += $"Meso {money} @ {reward.Proc:P0}";
        else
            line += "Unknown";

        var conditions = new List<string>();
        if (reward.ReqQuest.HasValue)
            conditions.Add($"Quest:{reward.ReqQuest}");
        if (reward.ReqLevelMin.HasValue || reward.ReqLevelMax.HasValue)
            conditions.Add($"Lv:{reward.ReqLevelMin ?? 0}-{reward.ReqLevelMax ?? 999}");
        if (reward.ReqMobLevelMin.HasValue || reward.ReqMobLevelMax.HasValue)
            conditions.Add($"MobLv:{reward.ReqMobLevelMin ?? 0}-{reward.ReqMobLevelMax ?? 999}");
        if (reward.DateStart.HasValue || reward.DateEnd.HasValue)
            conditions.Add("TimeLimited");

        if (conditions.Count > 0)
            line += $" [{string.Join(", ", conditions)}]";

        return line + "\\r\\n";
    }

    private async Task ExecuteClear(IFieldUser user, DropCommandArgs args)
    {
        if (args.MobId is not { } mobId)
        {
            await user.Message("Usage: /drop clear <mobId>");
            return;
        }

        var pool = await _rewardPool.Retrieve(mobId);
        if (pool == null)
        {
            await user.Message($"No synthetic drops for mob {mobId}");
            return;
        }

        var rewards = await pool.RetrieveAll();
        var count = rewards.Count;
        foreach (var reward in rewards.ToList())
        {
            await pool.Delete(reward);
        }

        await user.Message($"Cleared {count} synthetic drops from mob {mobId}");
    }

    private async Task ExecuteClearGlobal(IFieldUser user)
    {
        var rewards = await _rewardPool.Global.RetrieveAll();
        var count = rewards.Count;
        foreach (var reward in rewards.ToList())
        {
            await _rewardPool.Global.Delete(reward);
        }

        await user.Message($"Cleared {count} global synthetic drops");
    }

    private async Task ExecuteRandom(IFieldUser user, DropCommandArgs args)
    {
        if (args.MobId is not { } targetArg)
        {
            await user.Message("Usage: /drop random <mobId|0> [count] [rate%]");
            await user.Message("  mobId: specific mob template ID");
            await user.Message("  0: all mob types on current field");
            return;
        }

        var count = args.ItemId ?? 3;
        const double defaultRate = 1.0;
        var ratePercent = Math.Abs(args.Rate - defaultRate) < 0.001 ? 50 : (int)args.Rate;
        var rate = Math.Clamp(ratePercent / 100.0, 0.01, 1.0);

        var output = "#e#bRandom Drops Generated#n\\r\\n";

        if (targetArg == 0)
        {
            if (user.Field == null)
            {
                await user.Message("You must be in a field to target field mobs");
                return;
            }

            var mobPool = user.Field.GetPool(FieldObjectType.Mob);
            var mobs = (mobPool?.Objects ?? ImmutableArray<IFieldObject>.Empty)
                .OfType<IFieldMob>()
                .ToList();

            if (mobs.Count == 0)
            {
                await user.Message("No mobs found on this field");
                return;
            }

            var uniqueMobIds = mobs.Select(m => m.Template.ID).Distinct().OrderBy(x => x).ToList();

            foreach (var mobId in uniqueMobIds)
            {
                var generated = await GenerateRandomDropsForMob(mobId, count, rate);
                output += $"\\r\\n#eMob {mobId}:#n\\r\\n";
                foreach (var (itemId, money, dropRate, qty) in generated)
                {
                    if (itemId.HasValue)
                        output += $"  Item {itemId.Value} @ {dropRate:P0} x{qty}\\r\\n";
                    else if (money.HasValue)
                        output += $"  Meso {money.Value} @ {dropRate:P0}\\r\\n";
                }
            }

            output += $"\\r\\n#rTotal: {uniqueMobIds.Count} mobs, {count + 1} drops each#k";
        }
        else
        {
            var generated = await GenerateRandomDropsForMob(targetArg, count, rate);
            output += $"\\r\\n#eMob {targetArg}:#n\\r\\n";
            foreach (var (itemId, money, dropRate, qty) in generated)
            {
                if (itemId.HasValue)
                    output += $"  Item {itemId.Value} @ {dropRate:P0} x{qty}\\r\\n";
                else if (money.HasValue)
                    output += $"  Meso {money.Value} @ {dropRate:P0}\\r\\n";
            }
        }

        await user.Prompt(s => s.Say(output), default);
    }

    private async Task<List<(int? ItemId, int? Money, double Rate, int Qty)>> GenerateRandomDropsForMob(int mobId, int count, double rate)
    {
        var pool = await GetOrCreatePoolAsync(mobId);

        var itemRanges = new (int min, int max)[]
        {
            (2000000, 2000100),  // Consumables
            (2010000, 2010100),  // Pills
            (2020000, 2020060),  // Food
            (4000000, 4000500),  // Etc items
            (4010000, 4010007),  // Ores
            (4020000, 4020009),  // Jewels
        };

        var generated = new List<(int? ItemId, int? Money, double Rate, int Qty)>();

        for (var i = 0; i < count; i++)
        {
            var range = itemRanges[Random.Shared.Next(itemRanges.Length)];
            var itemId = Random.Shared.Next(range.min, range.max + 1);
            var dropRate = rate * (0.5 + Random.Shared.NextDouble());
            dropRate = Math.Clamp(dropRate, 0.01, 1.0);
            var qty = Random.Shared.Next(1, 4);

            var reward = new MobReward(GetNextRewardId(), dropRate)
            {
                ItemID = itemId,
                NumberMin = 1,
                NumberMax = qty
            };
            await pool.Insert(reward);
            generated.Add((itemId, null, dropRate, qty));
        }

        var mesoAmount = Random.Shared.Next(50, 500);
        var mesoReward = new MobReward(GetNextRewardId(), rate)
        {
            Money = mesoAmount
        };
        await pool.Insert(mesoReward);
        generated.Add((null, mesoAmount, rate, 1));

        return generated;
    }

    private async Task ExecuteSwap(IFieldUser user, DropCommandArgs args)
    {
        if (args.MobId is not { } mobId1 || args.ItemId is not { } mobId2)
        {
            await user.Message("Usage: /drop swap <mobId1> <mobId2>");
            await user.Message("  Swaps the entire drop tables between two mobs");
            return;
        }

        if (mobId1 == mobId2)
        {
            await user.Message("Cannot swap a mob's drops with itself");
            return;
        }

        var pool1 = await _rewardPool.Retrieve(mobId1);
        var pool2 = await _rewardPool.Retrieve(mobId2);

        var rewards1 = pool1 != null ? (await pool1.RetrieveAll()).ToList() : new List<IMobReward>();
        var rewards2 = pool2 != null ? (await pool2.RetrieveAll()).ToList() : new List<IMobReward>();

        var newPool1 = new RewardPool<IMobReward>(mobId1);
        var newPool2 = new RewardPool<IMobReward>(mobId2);

        foreach (var reward in rewards2)
            await newPool1.Insert(CloneReward(reward));

        foreach (var reward in rewards1)
            await newPool2.Insert(CloneReward(reward));

        await _rewardPool.Update(newPool1);
        await _rewardPool.Update(newPool2);

        await user.Message($"Swapped drops between mob {mobId1} ({rewards1.Count} drops) and mob {mobId2} ({rewards2.Count} drops)");
    }

    private async Task ExecuteCopy(IFieldUser user, DropCommandArgs args)
    {
        if (args.MobId is not { } fromMobId || args.ItemId is not { } toMobId)
        {
            await user.Message("Usage: /drop copy <fromMobId> <toMobId>");
            await user.Message("  Copies all drops from one mob to another");
            return;
        }

        if (fromMobId == toMobId)
        {
            await user.Message("Cannot copy a mob's drops to itself");
            return;
        }

        var fromPool = await _rewardPool.Retrieve(fromMobId);
        if (fromPool == null)
        {
            await user.Message($"No drops found for source mob {fromMobId}");
            return;
        }

        var fromRewards = (await fromPool.RetrieveAll()).ToList();
        var toPool = await GetOrCreatePoolAsync(toMobId);

        foreach (var reward in fromRewards)
            await toPool.Insert(CloneReward(reward));

        await user.Message($"Copied {fromRewards.Count} drops from mob {fromMobId} to mob {toMobId}");
    }

    private async Task ExecuteRemove(IFieldUser user, DropCommandArgs args)
    {
        if (args.MobId is not { } mobId || args.ItemId is not { } rewardId)
        {
            await user.Message("Usage: /drop remove <mobId> <rewardId>");
            await user.Message("  Use /drop list <mobId> to see reward IDs");
            return;
        }

        var pool = await _rewardPool.Retrieve(mobId);
        if (pool == null)
        {
            await user.Message($"No drops for mob {mobId}");
            return;
        }

        var reward = await pool.Retrieve(rewardId);
        if (reward == null)
        {
            await user.Message($"Reward #{rewardId} not found for mob {mobId}");
            return;
        }

        await pool.Delete(reward);

        var desc = reward.ItemID.HasValue ? $"Item {reward.ItemID}" : $"Meso {reward.Money}";
        await user.Message($"Removed #{rewardId} ({desc}) from mob {mobId}");
    }

    private async Task ExecuteTest(IFieldUser user, DropCommandArgs args)
    {
        if (args.MobId is not { } mobId)
        {
            await user.Message("Usage: /drop test <mobId> [iterations]");
            await user.Message("  Simulates drops without spawning them");
            return;
        }

        var iterations = args.ItemId ?? 100;
        iterations = Math.Clamp(iterations, 1, 10000);

        var pool = await _rewardPool.Retrieve(mobId);
        var globalPool = _rewardPool.Global;

        var mobRewards = pool != null ? (await pool.RetrieveAll()).ToList() : new List<IMobReward>();
        var globalRewards = (await globalPool.RetrieveAll()).ToList();
        var allRewards = mobRewards.Concat(globalRewards).ToList();

        if (allRewards.Count == 0)
        {
            await user.Message($"No drops configured for mob {mobId}");
            return;
        }

        var dropCounts = new Dictionary<int, int>();
        foreach (var r in allRewards)
            dropCounts[r.ID] = 0;

        for (var i = 0; i < iterations; i++)
        {
            foreach (var reward in allRewards)
            {
                if (Random.Shared.NextDouble() <= reward.Proc)
                    dropCounts[reward.ID]++;
            }
        }

        var output = $"#e#bDrop Simulation: Mob {mobId}#n\\r\\n";
        output += $"Iterations: {iterations}\\r\\n\\r\\n";

        output += "#eReward | Expected | Actual | Variance#n\\r\\n";
        foreach (var reward in allRewards)
        {
            var expected = reward.Proc * iterations;
            var actual = dropCounts[reward.ID];
            var variance = expected > 0 ? ((actual - expected) / expected * 100) : 0;
            var varianceStr = variance >= 0 ? $"+{variance:F1}%" : $"{variance:F1}%";

            string desc;
            if (reward.ItemID.HasValue)
                desc = $"Item {reward.ItemID}";
            else if (reward.Money.HasValue)
                desc = $"Meso {reward.Money}";
            else
                desc = "Unknown";

            output += $"#{reward.ID} {desc} @ {reward.Proc:P0}\\r\\n";
            output += $"  Expected: {expected:F1} | Actual: {actual} | {varianceStr}\\r\\n";
        }

        output += $"\\r\\n#rTotal rewards: {allRewards.Count} (mob: {mobRewards.Count}, global: {globalRewards.Count})#k";

        await user.Prompt(s => s.Say(output), default);
    }

    private static async Task ShowHelp(IFieldUser user)
    {
        var help = "#e#bDrop Command Help#n\\r\\n\\r\\n" +
                   "#eManage Drops:#n\\r\\n" +
                   "/drop add <mob> <item> [rate] [min] [max]\\r\\n" +
                   "/drop addmoney <mob> <amount> [rate]\\r\\n" +
                   "/drop global <item> [rate] [min] [max]\\r\\n" +
                   "/drop remove <mob> <rewardId>\\r\\n" +
                   "/drop clear <mob> | /drop clearglobal\\r\\n\\r\\n" +
                   "#eGenerate:#n\\r\\n" +
                   "/drop random <mob|0> [count] [rate]\\r\\n" +
                   "/drop copy <fromMob> <toMob>\\r\\n" +
                   "/drop swap <mob1> <mob2>\\r\\n\\r\\n" +
                   "#eDiagnostics:#n\\r\\n" +
                   "/drop list [mob|-1] - show drops\\r\\n" +
                   "/drop test <mob> [iterations] - simulate";

        await user.Prompt(s => s.Say(help), default);
    }
}

public class DropCommandArgs : CommandArgs
{
    [ArgPosition(0)]
    [ArgDescription("Action: add, addmoney, global, list, clear, clearglobal")]
    public string? Action { get; set; }

    [ArgPosition(1)]
    [ArgDescription("Mob ID (or Item ID for global)")]
    public int? MobId { get; set; }

    [ArgPosition(2)]
    [ArgDescription("Item ID (or amount for money, or rate for global)")]
    public int? ItemId { get; set; }

    [ArgPosition(3)]
    [ArgDescription("Drop rate 0.0-1.0 (or min qty for global)")]
    public double Rate { get; set; } = 1.0;

    [ArgPosition(4)]
    [ArgDescription("Min quantity (or max qty for global)")]
    public int? Min { get; set; }

    [ArgPosition(5)]
    [ArgDescription("Max quantity")]
    public int? Max { get; set; }
}
