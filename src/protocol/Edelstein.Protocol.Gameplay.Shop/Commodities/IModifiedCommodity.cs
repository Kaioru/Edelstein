using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay.Shop.Commodities;

public interface IModifiedCommodity : IIdentifiable<int>
{
    CommodityFlags Flags { get; }
    
    int? ItemID { get; }
    short? Count { get; }
    byte? Priority { get; }
    
    int? Price { get; }
    bool? Bonus { get; }
    
    short? Period { get; }
    short? ReqPOP { get; }
    short? ReqLevel { get; }
    
    int? MaplePoint { get; }
    int? Meso { get; }
    
    bool? ForPremiumUser { get; }
    byte? Gender { get; }
    
    bool? OnSale { get; }
    
    byte? Class { get; }
    byte? Limit { get; }

    short? PbCash { get; }
    short? PbPoint { get; }
    short? PbGift { get; }

    ICollection<int>? PackageSN { get; }
}
