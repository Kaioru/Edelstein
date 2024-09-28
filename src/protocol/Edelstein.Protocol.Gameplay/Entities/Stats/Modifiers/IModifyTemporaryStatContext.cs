using System;

namespace Edelstein.Protocol.Gameplay.Entities.Stats.Modifiers;

public interface IModifyTemporaryStatContext
{
    ITemporaryStats StatsReset { get; }
    ITemporaryStats StatsSet { get; }

    void Set(TemporaryStatType type, int option, int reason, DateTime? dateExpire = null);
    
    void SetEnergyCharged(int? option = null, int? reason = null, TimeSpan? term = null);
    void SetDashSpeed(int? option = null, int? reason = null, TimeSpan? term = null);
    void SetDashJump(int? option = null, int? reason = null, TimeSpan? term = null);
    void SetRideVehicle(int? option = null, int? reason = null);
    void SetPartyBooster(int? option = null, int? reason = null, DateTime? dateStart = null, TimeSpan? term = null);
    void SetGuidedBullet(int? option = null, int? reason = null, int? mobID = null);
    void SetUndead(int? option = null, int? reason = null, TimeSpan? term = null);
    
    void ResetEnergyCharged();
    void ResetDashSpeed();
    void ResetDashJump();
    void ResetRideVehicle();
    void ResetPartyBooster();
    void ResetGuidedBullet();
    void ResetUndead();
    
    void ResetByType(TemporaryStatType type);
    void ResetByReason(int reason);
    
    void ResetAll();
}
