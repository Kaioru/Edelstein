using System.Collections.Immutable;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Shop.Commodities;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Shop.Commodities.Templates;

public record ModifiedCommodityTemplate : IModifiedCommodity, ITemplate
{
    public ModifiedCommodityTemplate(int sn, IDataProperty property)
    {
        ID = sn;
        ItemID = property.Resolve<int>("ItemId");
        Count = property.Resolve<short>("Count");
        Priority = property.Resolve<byte>("Priority");
        
        Price = property.Resolve<int>("Price");
        Bonus = property.Resolve<int>("Bonus") > 0;
        
        Period = property.Resolve<short>("Period");
        ReqPOP = property.Resolve<short>("ReqPOP");
        ReqLevel = property.Resolve<short>("ReqLVL");
        
        MaplePoint = property.Resolve<int>("MaplePoint");
        Meso = property.Resolve<int>("Meso");
        
        ForPremiumUser = property.Resolve<int>("Premium") > 0;
        
        Gender = property.Resolve<byte>("Gender");
            
        OnSale = property.Resolve<int>("OnSale") > 0;
        Class = property.Resolve<byte>("Class");
        Limit = property.Resolve<byte>("Limit");
        
        PbCash = property.Resolve<short>("PbCash");
        PbPoint = property.Resolve<short>("PbPoint");
        PbGift = property.Resolve<short>("PbGift");

        if (ItemID != null) Flags |= CommodityFlags.ItemID;
        if (Count != null) Flags |= CommodityFlags.Count;
        if (Priority != null) Flags |= CommodityFlags.Priority;
        
        if (Price != null) Flags |= CommodityFlags.Price;
        if (Bonus != null) Flags |= CommodityFlags.Bonus;
        
        if (Period != null) Flags |= CommodityFlags.Period;
        if (ReqPOP != null) Flags |= CommodityFlags.ReqPOP;
        if (ReqLevel != null) Flags |= CommodityFlags.ReqLVL;
        
        if (MaplePoint != null) Flags |= CommodityFlags.MaplePoint;
        if (Meso != null) Flags |= CommodityFlags.Meso;
        
        if (ForPremiumUser != null) Flags |= CommodityFlags.ForPremiumUser;
        
        if (Gender != null) Flags |= CommodityFlags.CommodityGender;
        
        if (OnSale != null) Flags |= CommodityFlags.OnSale;
        if (Class != null) Flags |= CommodityFlags.Class;
        if (Limit != null) Flags |= CommodityFlags.Limit;
        
        if (PbCash != null) Flags |= CommodityFlags.PbCash;
        if (PbPoint != null) Flags |= CommodityFlags.PbPoint;
        if (PbGift != null) Flags |= CommodityFlags.PbGift;
    }
    
    public int ID { get; }
    
    public CommodityFlags Flags { get; }
    
    public int? ItemID { get; }
    public short? Count { get; }
    
    public byte? Priority { get; }
    public int? Price { get; }
    
    public bool? Bonus { get; }
    
    public short? Period { get; }
    public short? ReqPOP { get; }
    public short? ReqLevel { get; }
    
    public int? MaplePoint { get; }
    public int? Meso { get; }
    
    public bool? ForPremiumUser { get; }
    
    public byte? Gender { get; }
    
    public bool? OnSale { get; }
    public byte? Class { get; }
    public byte? Limit { get; }
    
    public short? PbCash { get; }
    public short? PbPoint { get; }
    public short? PbGift { get; }
    
    public ICollection<int>? PackageSN { get; }
}
