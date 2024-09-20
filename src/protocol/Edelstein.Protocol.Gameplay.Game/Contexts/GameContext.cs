using Duey.Abstractions;
using Edelstein.Protocol.Utilities;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContext(
    ITicker Ticker,
    IDataNamespace DataNamespace,
    IDateTimeProvider DateTime,
    GameContextManagers Managers,
    GameContextConversations Conversations,
    GameContextCalculators Calculators,
    GameContextTemplates Templates,
    GameContextPipelines Pipelines
);
