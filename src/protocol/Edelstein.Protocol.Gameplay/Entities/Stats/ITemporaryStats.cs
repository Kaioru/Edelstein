using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Entities.Stats.TwoState;

namespace Edelstein.Protocol.Gameplay.Entities.Stats;

public interface ITemporaryStats
{
    TemporaryStatOption? this[TemporaryStatType type] { get; }
    IDictionary<TemporaryStatType, TemporaryStatOption> Records { get; }
    
    TemporaryStatDiceInfo? DiceInfo { get; set; }
    
    TwoStateDynamicTermOption? EnergyCharged { get; set; }
    TwoStateDynamicTermOption? DashSpeed { get; set; }
    TwoStateDynamicTermOption? DashJump { get; set; }
    TwoStateOption? RideVehicle { get; set; }
    TwoStatePartyBoosterOption? PartyBooster { get; set; }
    TwoStateGuidedBulletOption? GuidedBullet { get; set; }
    TwoStateDynamicTermOption? Undead { get; set; }
}
