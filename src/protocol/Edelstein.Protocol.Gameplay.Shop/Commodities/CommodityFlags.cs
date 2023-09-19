namespace Edelstein.Protocol.Gameplay.Shop.Commodities;

[Flags]
public enum CommodityFlags
{
    ItemID = 0x1,
    Count = 0x2,
    Priority = 0x10,
    Price = 0x4,
    Bonus = 0x8,
    Period = 0x20,
    ReqPOP = 0x20000,
    ReqLVL = 0x40000,
    MaplePoint = 0x40,
    Meso = 0x80,
    ForPremiumUser = 0x100,
    CommodityGender = 0x200,
    OnSale = 0x400,
    Class = 0x800,
    Limit = 0x1000,
    PbCash = 0x2000,
    PbPoint = 0x4000,
    PbGift = 0x80000,
    PackageSN = 0x10000
}
