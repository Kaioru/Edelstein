using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Shop.Commodities.Templates;

namespace Edelstein.Common.Gameplay.Shop.Commodities.Templates;

public record CommodityTemplate : ICommodityTemplate
{
    public CommodityTemplate(IDataNode node)
    {
        ID = node.ResolveInt("SN") ?? 0;
        ItemID = node.ResolveInt("ItemId") ?? 0;
        Count = node.ResolveShort("Count") ?? 0;
        Priority = node.ResolveByte("Priority") ?? 0;
        
        Price = node.ResolveInt("Price") ?? 0;
        Bonus = (node.ResolveInt("Bonus") ?? 0) > 0;
        
        Period = node.ResolveShort("Period") ?? 0;
        ReqPOP = node.ResolveShort("ReqPOP") ?? 0;
        ReqLevel = node.ResolveShort("ReqLEV") ?? 0;
        
        MaplePoint = node.ResolveInt("MaplePoint") ?? 0;
        Meso = node.ResolveInt("Meso") ?? 0;
        
        ForPremiumUser = (node.ResolveInt("Premium") ?? 0) > 0;
        
        var gender = node.ResolveInt("Gender") ?? 0;
        if (gender == -1) gender = 2;
        Gender = (byte)gender;
            
        OnSale = (node.ResolveInt("OnSale") ?? 0) > 0;
        Class = node.ResolveByte("Class") ?? 0;
        Limit = node.ResolveByte("Limit") ?? 0;
        
        PbCash = node.ResolveShort("PbCash") ?? 0;
        PbPoint = node.ResolveShort("PbPoint") ?? 0;
        PbGift = node.ResolveShort("PbGift") ?? 0;
    }
    
    public int ID { get; }
    public int ItemID { get; }
    public short Count { get; }
    public byte Priority { get; }
    
    public int Price { get; }
    public bool Bonus { get; }
    
    public short Period { get; }
    public short ReqPOP { get; }
    public short ReqLevel { get; }
    
    public int MaplePoint { get; }
    public int Meso { get; }
    
    public bool ForPremiumUser { get; }
    public byte Gender { get; }
    
    public bool OnSale { get; }
    public byte Class { get; }
    public byte Limit { get; }
    
    public short PbCash { get; }
    public short PbPoint { get; }
    public short PbGift { get; }
}
