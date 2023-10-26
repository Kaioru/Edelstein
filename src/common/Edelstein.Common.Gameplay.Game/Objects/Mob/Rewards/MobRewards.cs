using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Rewards;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Rewards;

public class MobRewards
{
    public ICollection<IMobReward> Items { get; }
    
    public MobRewards() 
        => Items = new List<IMobReward>();
}
