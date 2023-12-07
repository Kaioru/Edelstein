using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Rewards;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob.Rewards;

public interface IMobRewardPoolManager : IRewardPoolManager<IMobReward>
{
    IRepository<int, IMobReward> Global { get; }

    Task<ICollection<IMobReward>> CalculateRewards(IFieldUser user, IFieldMob mob);
}
