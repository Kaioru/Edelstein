using Duey.Abstractions;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Protocol.Gameplay.Trade.Contexts;

public record TradeContextManagers(
    IDataNamespace Data,
    ITickerManager Ticker
);
