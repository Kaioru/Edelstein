using System;
using System.Collections.Frozen;
using System.Linq;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Gameplay.Entities.Stats;
using Edelstein.Protocol.Gameplay.Entities.Stats.Modifiers;
using Edelstein.Protocol.Gameplay.Entities.Stats.TwoState;

namespace Edelstein.Common.Gameplay.Entities.Modifiers;

public class ModifyTemporaryStatContext(
    ITemporaryStats stats
) : IModifyTemporaryStatContext
{
    public ITemporaryStats StatsReset { get; } = new CharacterTemporaryStats();
    public ITemporaryStats StatsSet { get; } = new CharacterTemporaryStats();
    
    public void Set(TemporaryStatType type, int option, int reason, DateTime? dateExpire = null)
    {
        var stat = new TemporaryStatOption
        {
            Option = option,
            Reason = reason,
            DateExpire = dateExpire
        };

        stats.Records[type] = stat;
        StatsSet.Records[type] = stat;
    }

    public void SetEnergyCharged(int? option = null, int? reason = null, TimeSpan? term = null)
    {
        var record = stats.EnergyCharged ?? new TwoStateDynamicTermOption();
        
        record.Option = option ?? record.Option;
        record.Reason = reason ?? record.Reason;
        record.DateUpdated = DateTime.UtcNow;
        record.Term = term ?? record.Term;
        
        stats.EnergyCharged = record;
        StatsSet.EnergyCharged = record;
    }
    
    public void SetDashSpeed(int? option = null, int? reason = null, TimeSpan? term = null)
    {
        var record = stats.DashSpeed ?? new TwoStateDynamicTermOption();
        
        record.Option = option ?? record.Option;
        record.Reason = reason ?? record.Reason;
        record.DateUpdated = DateTime.UtcNow;
        record.Term = term ?? record.Term;
        
        stats.DashSpeed = record;
        StatsSet.DashSpeed = record;
    }
    
    public void SetDashJump(int? option = null, int? reason = null, TimeSpan? term = null)
    {
        var record = stats.DashJump ?? new TwoStateDynamicTermOption();
        
        record.Option = option ?? record.Option;
        record.Reason = reason ?? record.Reason;
        record.DateUpdated = DateTime.UtcNow;
        record.Term = term ?? record.Term;
        
        stats.DashJump = record;
        StatsSet.DashJump = record;
    }

    public void SetRideVehicle(int? option = null, int? reason = null)
    {
        var record = stats.RideVehicle ?? new TwoStateOption();
        
        record.Option = option ?? record.Option;
        record.Reason = reason ?? record.Reason;
        record.DateUpdated = DateTime.UtcNow;
            
        stats.RideVehicle = record;
        StatsSet.RideVehicle = record;
    }
    
    public void SetPartyBooster(int? option = null, int? reason = null, DateTime? dateStart = null, TimeSpan? term = null)
    {
        var record = stats.PartyBooster ?? new TwoStatePartyBoosterOption();
        
        record.Option = option ?? record.Option;
        record.Reason = reason ?? record.Reason;
        record.DateUpdated = DateTime.UtcNow;
        record.DateStart = dateStart ?? record.DateStart;
        record.Term = term ?? record.Term;
        
        stats.PartyBooster = record;
        StatsSet.PartyBooster = record;
    }
    
    public void SetGuidedBullet(int? option = null, int? reason = null, int? mobID = null)
    {
        var record = stats.GuidedBullet ?? new TwoStateGuidedBulletOption();
        
        record.Option = option ?? record.Option;
        record.Reason = reason ?? record.Reason;
        record.MobID = mobID ?? record.MobID;
        record.DateUpdated = DateTime.UtcNow;
        
        stats.GuidedBullet = record;
        StatsSet.GuidedBullet = record;
    }
    
    public void SetUndead(int? option = null, int? reason = null, TimeSpan? term = null)
    {
        var record = stats.Undead ?? new TwoStateDynamicTermOption();
        
        record.Option = option ?? record.Option;
        record.Reason = reason ?? record.Reason;
        record.DateUpdated = DateTime.UtcNow;
        record.Term = term ?? record.Term;
        
        stats.Undead = record;
        StatsSet.Undead = record;
    }

    public void ResetEnergyCharged()
    {
        var record = stats.EnergyCharged ?? new TwoStateDynamicTermOption();

        record.Option = 0;
        record.Reason = 0;
        record.DateUpdated = DateTime.UtcNow;
        record.Term = TimeSpan.Zero;
        
        stats.EnergyCharged = record;
        StatsReset.EnergyCharged = record;
    }
    
    public void ResetDashSpeed()
    {
        var record = stats.DashSpeed ?? new TwoStateDynamicTermOption();

        record.Option = 0;
        record.Reason = 0;
        record.DateUpdated = DateTime.UtcNow;
        record.Term = TimeSpan.Zero;
        
        stats.DashSpeed = record;
        StatsReset.DashSpeed = record;
    }
    
    public void ResetDashJump()
    {
        var record = stats.DashJump ?? new TwoStateDynamicTermOption();

        record.Option = 0;
        record.Reason = 0;
        record.DateUpdated = DateTime.UtcNow;
        record.Term = TimeSpan.Zero;
        
        stats.DashJump = record;
        StatsReset.DashJump = record;
    }
    
    public void ResetRideVehicle()
    {
        var record = stats.RideVehicle ?? new TwoStateOption();

        record.Option = 0;
        record.Reason = 0;
        record.DateUpdated = DateTime.UtcNow;
        
        stats.RideVehicle = record;
        StatsReset.RideVehicle = record;
    }
    public void ResetPartyBooster()
    {
        var record = stats.PartyBooster ?? new TwoStatePartyBoosterOption();

        record.Option = 0;
        record.Reason = 0;
        record.DateUpdated = DateTime.UtcNow;
        record.DateStart = DateTime.MinValue;
        record.Term = TimeSpan.Zero;
        
        stats.PartyBooster = record;
        StatsReset.PartyBooster = record;
    }
    
    public void ResetGuidedBullet()
    {
        var record = stats.GuidedBullet ?? new TwoStateGuidedBulletOption();

        record.Option = 0;
        record.Reason = 0;
        record.MobID = 0;
        record.DateUpdated = DateTime.UtcNow;
        
        stats.GuidedBullet = record;
        StatsReset.GuidedBullet = record;
    }

    public void ResetUndead()
    {
        var record = stats.Undead ?? new TwoStateDynamicTermOption();

        record.Option = 0;
        record.Reason = 0;
        record.DateUpdated = DateTime.UtcNow;
        record.Term = TimeSpan.Zero;
        
        stats.Undead = record;
        StatsReset.Undead = record;
    }
   
    public void ResetByType(TemporaryStatType type)
    {
        if (stats.Records.TryGetValue(type, out var record))
            StatsReset.Records[type] = record;
        stats.Records.Remove(type);
    }

    public void ResetByReason(int reason)
    {
        foreach (var type in stats.Records
                     .Where(kv => kv.Value.Reason == reason)
                     .Select(kv => kv.Key)
                     .ToFrozenSet())
            ResetByType(type);
    }
    
    public void ResetAll()
    {
        foreach (var type in stats.Records.Keys)
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
