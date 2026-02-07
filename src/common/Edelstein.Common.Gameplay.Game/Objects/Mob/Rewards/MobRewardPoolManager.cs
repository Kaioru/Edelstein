using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Game.Rates;
using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Rewards;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Rates;
using Edelstein.Protocol.Gameplay.Game.Rewards;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Rewards;

public class MobRewardPoolManager :
    Repository<int, IRewardPool<IMobReward>>,
    IMobRewardPoolManager
{
    public IRepository<int, IMobReward> Global { get; } = new Repository<int, IMobReward>();

    public async Task<ICollection<IMobReward>> CalculateRewards(IFieldUser user, IFieldMob mob)
    {
        var pool = await Retrieve(mob.Template.ID);
        var rates = user.StageUser.Context.Managers.Rates;
        var rateContext = new RateContext(user, user.StageUser.Context.Options);
        var dropRate = await rates.GetFinalRateAsync(RateType.Drop, rateContext);

        return (pool != null ? await pool.RetrieveAll() : [])
            .Concat(await Global.RetrieveAll())
            .Where(r =>
            {
                // TODO: add filters (quest requirements, etc.)
                // NOTE: drop rate is clamped at 100%. TODO: consider overflow behaviors
                var proc = Math.Min(r.Proc * dropRate, 1d);
                return Random.Shared.NextDouble() <= proc;
            })
            .OrderBy(_ => Random.Shared.Next())
            .ToImmutableArray();
    }
}
