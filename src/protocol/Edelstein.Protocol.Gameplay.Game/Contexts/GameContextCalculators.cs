using Edelstein.Protocol.Gameplay.Game.Objects.Users.Stats;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextCalculators(
    IFieldUserStatsCalculator UserStats
);
