using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Characters.Stats;

namespace Edelstein.Common.Gameplay.Models.Characters;

public class CharacterTemporaryStats : ICharacterTemporaryStats
{
    public ITemporaryStatRecord? this[TemporaryStatType type] => Records.TryGetValue(type, out var record) ? record : null;
    public IDictionary<TemporaryStatType, ITemporaryStatRecord> Records { get; } = new Dictionary<TemporaryStatType, ITemporaryStatRecord>();
}
