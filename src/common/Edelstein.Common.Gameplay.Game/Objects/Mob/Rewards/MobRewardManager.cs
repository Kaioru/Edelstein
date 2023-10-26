using System.Collections.Immutable;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Rewards;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Rewards;

public class MobRewardManager : IMobRewardManager
{
    private readonly IDictionary<int, MobRewards> _rewards;
    private readonly ICollection<IMobReward> _rewardsAll;

    public MobRewardManager()
    {
        _rewards = new Dictionary<int, MobRewards>();
        _rewardsAll = new List<IMobReward>();
    }

    public Task Insert(int mobID, IMobReward reward)
    {
        if (!_rewards.TryGetValue(mobID, out var rewards))
        {
            rewards = new MobRewards();
            _rewards[mobID] = rewards;
        }

        rewards.Items.Add(reward);
        return Task.CompletedTask;
    }

    public Task InsertAll(IMobReward reward)
    {
        _rewardsAll.Add(reward);
        return Task.CompletedTask;
    }

    public Task<ICollection<IMobReward>> RetrieveAll(int mobID)
        => Task.FromResult<ICollection<IMobReward>>(_rewardsAll
            .Concat(_rewards.TryGetValue(mobID, out var rewards) ? rewards.Items : ImmutableArray<IMobReward>.Empty)
            .ToImmutableArray());

    public async Task<ICollection<IMobReward>> RetrieveAvailable(IFieldUser user, IFieldMob mob)
        => (await RetrieveAll(mob.Template.ID))
            .Where(r => true) // TODO
            .ToImmutableArray();
}
