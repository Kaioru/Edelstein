namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob.Stats;

public interface IFieldMobStatsCalculator
{
    Task<IFieldMobStats> Calculate(IFieldMob mob);
}
