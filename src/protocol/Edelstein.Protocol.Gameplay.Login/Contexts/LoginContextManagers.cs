using Edelstein.Protocol.Data;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Protocol.Gameplay.Login.Contexts;

public record LoginContextManagers(
    IDataManager Data,
    ITickerManager Ticker
);
