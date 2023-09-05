namespace Edelstein.Protocol.Gameplay.Models.Characters.Stats;

public interface ITemporaryStats
{
    ITemporaryStatRecord? this[TemporaryStatType type] { get; }
    IDictionary<TemporaryStatType, ITemporaryStatRecord> Records { get; }
}
