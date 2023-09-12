using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats.TwoState;

namespace Edelstein.Common.Gameplay.Models.Characters;

public class CharacterTemporaryStats : ICharacterTemporaryStats
{
    public ITemporaryStatRecord? this[TemporaryStatType type] => Records.TryGetValue(type, out var record) ? record : null;
    public IDictionary<TemporaryStatType, ITemporaryStatRecord> Records { get; } = new Dictionary<TemporaryStatType, ITemporaryStatRecord>();
    
    public ITwoStateTemporaryStatRecordDynamicTerm? EnergyChargedRecord { get; set; }
    public ITwoStateTemporaryStatRecordDynamicTerm? DashSpeedRecord { get; set; }
    public ITwoStateTemporaryStatRecordDynamicTerm? DashJumpRecord { get; set; }
    public ITwoStateTemporaryStatRecord? RideVehicleRecord { get; set; }
    public ITwoStatePartyBoosterRecord? PartyBoosterRecord { get; set; }
    public ITwoStateGuidedBulletRecord? GuidedBulletRecord { get; set; }
    public ITwoStateTemporaryStatRecordDynamicTerm? UndeadRecord { get; set; }
}
