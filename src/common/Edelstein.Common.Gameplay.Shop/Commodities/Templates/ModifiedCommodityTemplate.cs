using System.Collections.Frozen;
using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Shop.Commodities;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Shop.Commodities.Templates;

public record ModifiedCommodityTemplate : IModifiedCommodity, ITemplate
{
    public ModifiedCommodityTemplate(int sn, IDataNode node)
    {
        ID = sn;
        ItemID = node.ResolveInt("ItemId");
        Count = node.ResolveShort("Count");
        Priority = node.ResolveByte("Priority");
        
        Price = node.ResolveInt("Price");
        Bonus = node.ResolveInt("Bonus") > 0;
        
        Period = node.ResolveShort("Period");
        ReqPOP = node.ResolveShort("ReqPOP");
        ReqLevel = node.ResolveShort("ReqLEV");
        
        MaplePoint = node.ResolveInt("MaplePoint");
        Meso = node.ResolveInt("Meso");
        
        ForPremiumUser = node.ResolveInt("Premium") > 0;
        
        Gender = node.ResolveByte("Gender");
            
        OnSale = node.ResolveInt("OnSale") > 0;
        Class = node.ResolveByte("Class");
        Limit = node.ResolveByte("Limit");
        
        PbCash = node.ResolveShort("PbCash");
        PbPoint = node.ResolveShort("PbPoint");
        PbGift = node.ResolveShort("PbGift");
        
        PackageSN = node
            .ResolvePath("PackageSN")?
            .Select(c => c.ResolveInt() ?? 0)
            .ToFrozenSet() ?? null;

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
        
        if (PackageSN != null) Flags |= CommodityFlags.PackageSN;
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
