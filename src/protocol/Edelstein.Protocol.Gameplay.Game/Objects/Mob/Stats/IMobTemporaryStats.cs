namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;

public interface IMobTemporaryStats
{
    IMobTemporaryStatRecord? this[MobTemporaryStatType type] { get; }
    IDictionary<MobTemporaryStatType, IMobTemporaryStatRecord> Records { get; }
    ICollection<IMobBurnedInfo> BurnedInfo { get; }
}
