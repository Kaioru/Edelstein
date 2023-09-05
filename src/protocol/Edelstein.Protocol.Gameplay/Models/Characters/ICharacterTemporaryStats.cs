using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Protocol.Gameplay.Models.Characters;

public interface ICharacterTemporaryStats
{
    ITemporaryStatRecord? this[TemporaryStatType type] { get; }
    IDictionary<TemporaryStatType, ITemporaryStatRecord> Records { get; }
}
