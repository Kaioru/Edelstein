using Duey.Abstractions;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Protocol.Gameplay.Login.Contexts;

public record LoginContextManagers(
    IDataNamespace Data,
    ITickerManager Ticker
);
