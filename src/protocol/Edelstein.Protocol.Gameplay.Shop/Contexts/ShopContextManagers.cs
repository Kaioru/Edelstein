using Edelstein.Protocol.Data;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Protocol.Gameplay.Shop.Contexts;

public record ShopContextManagers(
    IDataManager Data,
    ITickerManager Ticker
);
