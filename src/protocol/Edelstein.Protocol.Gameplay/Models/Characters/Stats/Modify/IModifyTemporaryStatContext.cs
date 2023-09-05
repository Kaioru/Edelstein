namespace Edelstein.Protocol.Gameplay.Models.Characters.Stats.Modify;

public interface IModifyTemporaryStatContext
{
    ICharacterTemporaryStats HistoryReset { get; }
    ICharacterTemporaryStats HistorySet { get; }

    void Set(TemporaryStatType type, ITemporaryStatRecord stat);
    void Set(TemporaryStatType type, int value, int reason, DateTime? dateExpire = null);

    void ResetByType(TemporaryStatType type);
    void ResetByReason(int reason);
    void ResetAll();
}
