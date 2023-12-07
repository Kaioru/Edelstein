using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Gameplay.Game.Rewards;

namespace Edelstein.Common.Gameplay.Game.Rewards;

public class RewardPoolManager<TReward> : 
    Repository<int, IRewardPool<TReward>>,
    IRewardPoolManager<TReward>
    where TReward : IReward;
