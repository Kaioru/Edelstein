using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContext(
    ITicker Ticker,
    GameContextTemplates Templates,
    GameContextPipelines Pipelines
);
