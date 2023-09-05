namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats.Modify;

public interface IModifyMobTemporaryStatContext
{
    IMobTemporaryStats HistoryReset { get; }
    IMobTemporaryStats HistorySet { get; }

    void Set(MobTemporaryStatType type, IMobTemporaryStatRecord stat);
    void Set(MobTemporaryStatType type, int value, int reason, DateTime? dateExpire = null);

    void ResetByType(MobTemporaryStatType type);
    void ResetByReason(int reason);
    void ResetAll();
}
