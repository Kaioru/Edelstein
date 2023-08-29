namespace Edelstein.Protocol.Gameplay.Game.Objects.User.Stats;

public interface IFieldUserStatsCalculator
{
    Task<IFieldUserStats> Calculate(IFieldUser user);
}
