using Edelstein.Protocol.Gameplay.Game.Rewards;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob.Rewards;

public interface IMobReward : IReward
{
    int? ItemID { get; }
    
    int? Money { get; }
    
    int? NumberMin { get; }
    int? NumberMax { get; }
    
    int? ReqQuest { get; }
    
    int? ReqLevelMin { get; }
    int? ReqLevelMax { get; }
    
    int? ReqMobLevelMin { get; }
    int? ReqMobLevelMax { get; }
    
    DateTime? DateStart { get; }
    DateTime? DateEnd { get; }
    
    double Proc { get; }
}
