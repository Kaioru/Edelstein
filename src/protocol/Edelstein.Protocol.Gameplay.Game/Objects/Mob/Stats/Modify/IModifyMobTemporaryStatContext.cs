namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats.Modify;

public interface IModifyMobTemporaryStatContext
{
    IMobTemporaryStats HistoryReset { get; }
    IMobTemporaryStats HistorySet { get; }

    void Set(MobTemporaryStatType type, IMobTemporaryStatRecord stat);
    void Set(MobTemporaryStatType type, int value, int reason, DateTime? dateExpire = null);

    void SetBurnedInfo(IMobBurnedInfo info);

    void ResetByType(MobTemporaryStatType type);
    void ResetByReason(int reason);
    void ResetAll();

    void ResetBurnedInfo(IMobBurnedInfo info);
    void ResetBurnedInfoByCharacter(int characterID);
    void ResetBurnedInfoBySkill(int skillID);
    void ResetBurnedInfoAll();
}
