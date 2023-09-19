using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Shop.Commodities;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Protocol.Gameplay.Shop.Contexts;

public record ShopContextManagers(
    IDataManager Data,
    ITickerManager Ticker,
    
    INotSaleManager NotSale,
    ICommodityManager Commodity,
    IModifiedCommodityManager ModifiedCommodity
);
