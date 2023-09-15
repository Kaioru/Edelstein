using Edelstein.Protocol.Gameplay.Shop.Commodities;

namespace Edelstein.Common.Gameplay.Shop.Commodities;

public record Commodity : ICommodity
{
    public int ID { get; set; }
    public int ItemID { get; set; }
    public short Count { get; set; }
    public byte Priority { get; set; }
    
    public int Price { get; set; }
    public bool Bonus { get; set; }
    
    public short Period { get; set; }
    public short ReqPOP { get; set; }
    public short ReqLevel { get; set; }
    
    public int MaplePoint { get; set; }
    public int Meso { get; set; }
    
    public bool ForPremiumUser { get; set; }
    public byte Gender { get; set; }
    
    public bool OnSale { get; set; }
    public byte Class { get; set; }
    public byte Limit { get; set; }
    
    public short PbCash { get; set; }
    public short PbPoint { get; set; }
    public short PbGift { get; set; }
}
