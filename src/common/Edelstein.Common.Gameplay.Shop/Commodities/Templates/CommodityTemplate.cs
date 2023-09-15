using System.Collections.Immutable;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Shop.Commodities.Templates;

namespace Edelstein.Common.Gameplay.Shop.Commodities.Templates;

public record CommodityTemplate : ICommodityTemplate
{
    public CommodityTemplate(IDataProperty property)
    {
        ID = property.Resolve<int>("SN") ?? 0;
        ItemID = property.Resolve<int>("ItemId") ?? 0;
        Count = property.Resolve<short>("Count") ?? 0;
        Priority = property.Resolve<byte>("Priority") ?? 0;
        
        Price = property.Resolve<int>("Price") ?? 0;
        Bonus = (property.Resolve<int>("Bonus") ?? 0) > 0;
        
        Period = property.Resolve<short>("Period") ?? 0;
        ReqPOP = property.Resolve<short>("ReqPOP") ?? 0;
        ReqLevel = property.Resolve<short>("ReqLVL") ?? 0;
        
        MaplePoint = property.Resolve<int>("MaplePoint") ?? 0;
        Meso = property.Resolve<int>("Meso") ?? 0;
        
        ForPremiumUser = (property.Resolve<int>("Premium") ?? 0) > 0;
        
        var gender = property.Resolve<int>("Gender") ?? 0;
        if (gender == -1) gender = 2;
        Gender = (byte)gender;
            
        OnSale = (property.Resolve<int>("OnSale") ?? 0) > 0;
        Class = property.Resolve<byte>("Class") ?? 0;
        Limit = property.Resolve<byte>("Limit") ?? 0;
        
        PbCash = property.Resolve<short>("PbCash") ?? 0;
        PbPoint = property.Resolve<short>("PbPoint") ?? 0;
        PbGift = property.Resolve<short>("PbGift") ?? 0;
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
