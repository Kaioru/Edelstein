using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Rewards;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Rewards;

public record MobReward : IMobReward
{
    public int? ItemID { get; init; }
    
    public int? Money { get; init; }
    
    public int? NumberMin { get; init; }
    public int? NumberMax { get; init; }
    
    public int? ReqQuest { get; init; }
    
    public int? ReqLevelMin { get; init; }
    public int? ReqLevelMax { get; init; }
    
    public int? ReqMobLevelMin { get; init; }
    public int? ReqMobLevelMax { get; init; }
    
    public DateTime? DateStart { get; init; }
    public DateTime? DateEnd { get; init; }
}
