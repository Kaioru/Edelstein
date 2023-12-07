using System.Collections.Immutable;
using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Rewards;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
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
        var random = new Random();
        var pool = await Retrieve(mob.Template.ID);
        
        return 
            (pool != null 
                ? await pool.RetrieveAll()
                : new List<IMobReward>())
            .Concat(await Global.RetrieveAll())
            .Where(r =>
            {
                // TODO filters
                
                return random.NextSingle() <= r.Proc;
            })
            .OrderBy(i => Random.Shared.Next())
            .ToImmutableArray();
    }
}
