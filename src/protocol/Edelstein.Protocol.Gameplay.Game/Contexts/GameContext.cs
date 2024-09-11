using Edelstein.Protocol.Utilities;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContext(
    ITicker Ticker,
    IDateTimeProvider DateTime,
    GameContextManagers Managers,
    GameContextCalculators Calculators,
    GameContextTemplates Templates,
    GameContextPipelines Pipelines
);
