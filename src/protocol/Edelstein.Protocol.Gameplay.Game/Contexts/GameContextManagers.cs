using Edelstein.Protocol.Data;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextManagers(
    IDataManager Data,
    ITickerManager Ticker
);
