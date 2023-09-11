namespace Edelstein.Protocol.Gameplay.Models.Characters.Stats.Modify;

public interface IModifyTemporaryStatContext
{
    ICharacterTemporaryStats HistoryReset { get; }
    ICharacterTemporaryStats HistorySet { get; }

    void Set(TemporaryStatType type, ITemporaryStatRecord stat);
    void Set(TemporaryStatType type, int value, int reason, DateTime? dateExpire = null);

    void SetEnergyCharged(int? value = null, int? reason = null, TimeSpan? term = null);
    void SetDashSpeed(int? value = null, int? reason = null, TimeSpan? term = null);
    void SetDashJump(int? value = null, int? reason = null, TimeSpan? term = null);
    void SetRideVehicle(int? value = null, int? reason = null);
    void SetPartyBooster(int? value = null, int? reason = null, DateTime? dateStart = null, TimeSpan? term = null);
    void SetGuidedBullet(int? value = null, int? reason = null, int? mobID = null);
    void SetUndead(int? value = null, int? reason = null, TimeSpan? term = null);

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
