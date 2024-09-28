using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Entities.Stats;
using Edelstein.Protocol.Gameplay.Entities.Stats.TwoState;

namespace Edelstein.Protocol.Gameplay.Entities;

public record CharacterTemporaryStats : ITemporaryStats
{
    public TemporaryStatOption? this[TemporaryStatType type] 
        => Records.TryGetValue(type, out var record) ? record : null;
    public IDictionary<TemporaryStatType, TemporaryStatOption> Records { get; } 
        = new Dictionary<TemporaryStatType, TemporaryStatOption>();

    public TemporaryStatDiceInfo? DiceInfo { get; set; }
    
    public TwoStateDynamicTermOption? EnergyCharged { get; set; }
    public TwoStateDynamicTermOption? DashSpeed { get; set; }
    public TwoStateDynamicTermOption? DashJump { get; set; }
    public TwoStateOption? RideVehicle { get; set; }
    public TwoStatePartyBoosterOption? PartyBooster { get; set; }
    public TwoStateGuidedBulletOption? GuidedBullet { get; set; }
    public TwoStateDynamicTermOption? Undead { get; set; }
}
