using Edelstein.Protocol.Gameplay.Models.Characters.Stats.TwoState;

namespace Edelstein.Protocol.Gameplay.Models.Characters.Stats;

public interface ITemporaryStats
{
    ITemporaryStatRecord? this[TemporaryStatType type] { get; }
    IDictionary<TemporaryStatType, ITemporaryStatRecord> Records { get; }
    
    ITemporaryStatDiceInfo DiceInfo { get; }
    
    ITwoStateTemporaryStatRecordDynamicTerm? EnergyChargedRecord { get; set; }
    ITwoStateTemporaryStatRecordDynamicTerm? DashSpeedRecord { get; set; }
    ITwoStateTemporaryStatRecordDynamicTerm? DashJumpRecord { get; set; }
    ITwoStateTemporaryStatRecord? RideVehicleRecord { get; set; }
    ITwoStatePartyBoosterRecord? PartyBoosterRecord { get; set; }
    ITwoStateGuidedBulletRecord? GuidedBulletRecord { get; set; }
    ITwoStateTemporaryStatRecordDynamicTerm? UndeadRecord { get; set; }
}
