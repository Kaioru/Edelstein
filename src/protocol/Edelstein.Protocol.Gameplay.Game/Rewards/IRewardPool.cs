using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Game.Rewards;

public interface IRewardPool<TReward> : 
    IIdentifiable<int>,
    IRepository<int, TReward> 
    where TReward : IReward;
