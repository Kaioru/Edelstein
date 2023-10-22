namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob.Rewards;

public interface IMobReward
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
}
