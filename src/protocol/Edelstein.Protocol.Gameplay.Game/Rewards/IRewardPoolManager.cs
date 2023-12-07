using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Game.Rewards;

public interface IRewardPoolManager<TReward> :
    IRepository<int, IRewardPool<TReward>> 
    where TReward : IReward;
