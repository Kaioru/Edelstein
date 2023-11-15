using System.Collections.Frozen;
using Edelstein.Common.Gameplay.Models.Characters.Stats.TwoState;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats.Modify;

namespace Edelstein.Common.Gameplay.Models.Characters.Stats.Modify;

public class ModifyTemporaryStatContext : IModifyTemporaryStatContext
{
    private readonly ICharacterTemporaryStats _stats;
    
    public ModifyTemporaryStatContext(ICharacterTemporaryStats stats)
    {
        _stats = stats;
        HistoryReset = new CharacterTemporaryStats();
        HistorySet = new CharacterTemporaryStats();
    }
        
    public ICharacterTemporaryStats HistoryReset { get; }
    public ICharacterTemporaryStats HistorySet { get; }

    public void Set(TemporaryStatType type, ITemporaryStatRecord stat)
    {
        ResetByType(type);

        _stats.Records[type] = stat;
        HistorySet.Records[type] = stat;
    }

    public void Set(TemporaryStatType type, int value, int reason, DateTime? dateExpire = null)
        => Set(type, new TemporaryStatRecord
        {
            Value = value,
            Reason = reason,
            DateExpire = dateExpire
        });
    
    public void SetEnergyCharged(int? value = null, int? reason = null, TimeSpan? term = null)
    {
        var record = _stats.EnergyChargedRecord ?? new TwoStateEnergyChargedRecord();
        
        record.Value = value ?? record.Value;
        record.Reason = reason ?? record.Reason;
        record.DateUpdated = DateTime.UtcNow;
        record.Term = term ?? record.Term;
        
        _stats.EnergyChargedRecord = record;
        HistorySet.EnergyChargedRecord = record;
    }
    
    public void SetDashSpeed(int? value = null, int? reason = null, TimeSpan? term = null)
    {
        var record = _stats.DashSpeedRecord ?? new TwoStateTemporaryStatRecordDynamicTerm();
        
        record.Value = value ?? record.Value;
        record.Reason = reason ?? record.Reason;
        record.DateUpdated = DateTime.UtcNow;
        record.Term = term ?? record.Term;
        
        _stats.DashSpeedRecord = record;
        HistorySet.DashSpeedRecord = record;
    }

    public void SetDashJump(int? value = null, int? reason = null, TimeSpan? term = null)
    {
        var record = _stats.DashJumpRecord ?? new TwoStateTemporaryStatRecordDynamicTerm();
        
        record.Value = value ?? record.Value;
        record.Reason = reason ?? record.Reason;
        record.DateUpdated = DateTime.UtcNow;
        record.Term = term ?? record.Term;
        
        _stats.DashJumpRecord = record;
        HistorySet.DashJumpRecord = record;
    }

    public void SetRideVehicle(int? value = null, int? reason = null)
    {
        var record = _stats.RideVehicleRecord ?? new TwoStateTemporaryStatRecord();
        
        record.Value = value ?? record.Value;
        record.Reason = reason ?? record.Reason;
        record.DateUpdated = DateTime.UtcNow;
            
        _stats.RideVehicleRecord = record;
        HistorySet.RideVehicleRecord = record;
    }
    public void SetPartyBooster(int? value = null, int? reason = null, DateTime? dateStart = null, TimeSpan? term = null)
    {
        var record = _stats.PartyBoosterRecord ?? new TwoStatePartyBoosterRecord();
        
        record.Value = value ?? record.Value;
        record.Reason = reason ?? record.Reason;
        record.DateUpdated = DateTime.UtcNow;
        record.DateStart = dateStart ?? record.DateStart;
        record.Term = term ?? record.Term;
        
        _stats.PartyBoosterRecord = record;
        HistorySet.PartyBoosterRecord = record;
    }
    
    public void SetGuidedBullet(int? value = null, int? reason = null, int? mobID = null)
    {
        var record = _stats.GuidedBulletRecord ?? new TwoStateGuidedBulletRecord();
        
        record.Value = value ?? record.Value;
        record.Reason = reason ?? record.Reason;
        record.MobID = mobID ?? record.MobID;
        record.DateUpdated = DateTime.UtcNow;
        
        _stats.GuidedBulletRecord = record;
        HistorySet.GuidedBulletRecord = record;
    }

    public void SetUndead(int? value = null, int? reason = null, TimeSpan? term = null)
    {
        var record = _stats.UndeadRecord ?? new TwoStateTemporaryStatRecordDynamicTerm();
        
        record.Value = value ?? record.Value;
        record.Reason = reason ?? record.Reason;
        record.DateUpdated = DateTime.UtcNow;
        record.Term = term ?? record.Term;
        
        _stats.UndeadRecord = record;
        HistorySet.UndeadRecord = record;
    }
    
    public void ResetEnergyCharged()
    {
        var record = _stats.EnergyChargedRecord ?? new TwoStateEnergyChargedRecord();

        record.Value = 0;
        record.Reason = 0;
        record.DateUpdated = DateTime.UtcNow;
        record.Term = TimeSpan.Zero;
        
        _stats.EnergyChargedRecord = record;
        HistoryReset.EnergyChargedRecord = record;
    }
    
    public void ResetDashSpeed()
    {
        var record = _stats.DashSpeedRecord ?? new TwoStateTemporaryStatRecordDynamicTerm();

        record.Value = 0;
        record.Reason = 0;
        record.DateUpdated = DateTime.UtcNow;
        record.Term = TimeSpan.Zero;
        
        _stats.DashSpeedRecord = record;
        HistoryReset.DashSpeedRecord = record;
    }
    
    public void ResetDashJump()
    {
        var record = _stats.DashJumpRecord ?? new TwoStateTemporaryStatRecordDynamicTerm();

        record.Value = 0;
        record.Reason = 0;
        record.DateUpdated = DateTime.UtcNow;
        record.Term = TimeSpan.Zero;
        
        _stats.DashJumpRecord = record;
        HistoryReset.DashJumpRecord = record;
    }
    
    public void ResetRideVehicle()
    {
        var record = _stats.RideVehicleRecord ?? new TwoStateTemporaryStatRecord();

        record.Value = 0;
        record.Reason = 0;
        record.DateUpdated = DateTime.UtcNow;
        
        _stats.RideVehicleRecord = record;
        HistoryReset.RideVehicleRecord = record;
    }
    public void ResetPartyBooster()
    {
        var record = _stats.PartyBoosterRecord ?? new TwoStatePartyBoosterRecord();

        record.Value = 0;
        record.Reason = 0;
        record.DateUpdated = DateTime.UtcNow;
        record.DateStart = DateTime.MinValue;
        record.Term = TimeSpan.Zero;
        
        _stats.PartyBoosterRecord = record;
        HistoryReset.PartyBoosterRecord = record;
    }
    
    public void ResetGuidedBullet()
    {
        var record = _stats.GuidedBulletRecord ?? new TwoStateGuidedBulletRecord();

        record.Value = 0;
        record.Reason = 0;
        record.MobID = 0;
        record.DateUpdated = DateTime.UtcNow;
        
        _stats.GuidedBulletRecord = record;
        HistoryReset.GuidedBulletRecord = record;
    }

    public void ResetUndead()
    {
        var record = _stats.UndeadRecord ?? new TwoStateTemporaryStatRecordDynamicTerm();

        record.Value = 0;
        record.Reason = 0;
        record.DateUpdated = DateTime.UtcNow;
        record.Term = TimeSpan.Zero;
        
        _stats.UndeadRecord = record;
        HistoryReset.UndeadRecord = record;
    }

    public void ResetByType(TemporaryStatType type)
    {
        if (_stats.Records.ContainsKey(type))
            HistoryReset.Records[type] = _stats.Records[type];
        _stats.Records.Remove(type);
    }

    public void ResetByReason(int reason)
    {
        foreach (var type in _stats.Records
                     .Where(kv => kv.Value.Reason == reason)
                     .Select(kv => kv.Key)
                     .ToFrozenSet())
            ResetByType(type);
    }

    public void ResetAll()
    {
        foreach (var type in _stats.Records.Keys)
            ResetByType(type);
        ResetEnergyCharged();
        ResetDashSpeed();
        ResetDashJump();
        ResetRideVehicle();
        ResetPartyBooster();
        ResetGuidedBullet();
        ResetUndead();
    }
}
