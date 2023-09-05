using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Stats;

public class MobTemporaryStats : IMobTemporaryStats
{
    public IMobTemporaryStatRecord? this[MobTemporaryStatType type] => Records.TryGetValue(type, out var record) ? record : null;
    public IDictionary<MobTemporaryStatType, IMobTemporaryStatRecord> Records { get; } = new Dictionary<MobTemporaryStatType, IMobTemporaryStatRecord>();
}
