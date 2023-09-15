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
        // ReqPOP
        // ReqLVL
        // MaplePoint
        // Meso
        // ForPremiumUser
        
        Gender = property.Resolve<byte>("Gender");
            
        OnSale = property.Resolve<int>("OnSale") > 0;
        Class = property.Resolve<byte>("Class");
        Limit = property.Resolve<byte>("Limit");
        
        PbCash = property.Resolve<short>("PbCash");
        PbPoint = property.Resolve<short>("PbPoint");
        PbGift = property.Resolve<short>("PbGift");
        
        PackageSN = ImmutableList<int>.Empty;
    }
    
    public int ID { get; }
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
