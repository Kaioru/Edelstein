using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Gameplay.Game.Rewards;

namespace Edelstein.Common.Gameplay.Game.Rewards;

public class RewardPool<TReward> : 
    Repository<int, TReward>,
    IRewardPool<TReward>
    where TReward : IReward
{
    public int ID { get; }
    
    public RewardPool(int id) => ID = id;
}
