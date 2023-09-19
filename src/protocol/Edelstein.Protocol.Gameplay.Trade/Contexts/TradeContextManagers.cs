using Edelstein.Protocol.Data;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Protocol.Gameplay.Trade.Contexts;

public record TradeContextManagers(
    IDataManager Data,
    ITickerManager Ticker
);
