namespace Edelstein.Protocol.Gameplay.Trade.Contexts;

public record TradeContext(
    ITradeStageOptions Options,
    TradeContextManagers Managers,
    TradeContextServices Services,
    TradeContextRepositories Repositories,
    TradeContextTemplates Templates,
    TradeContextPipelines Pipelines
);
