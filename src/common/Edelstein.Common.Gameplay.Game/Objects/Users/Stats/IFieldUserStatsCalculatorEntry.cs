using Edelstein.Protocol.Gameplay.Game.Objects.Users.Stats;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Objects.Users.Stats;

public interface IFieldUserStatsCalculatorEntry : IPipe<IFieldUserStatsCalculatorContext>
{
    int Priority { get; }
}
