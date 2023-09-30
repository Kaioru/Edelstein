using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;

public record NPCShopTemplateItem : INPCShopItem
{
    public int Order { get; }
    
    public int TemplateID { get; }
    
    public int? Price { get; }
    public byte? DiscountRate { get; }
    
    public int? TokenTemplateID { get; }
    public int? TokenPrice { get; }
    
    public int? ItemPeriod { get; }
    public int? LevelLimited { get; }
    
    public double? UnitPrice { get; }
    public short? MaxPerSlot { get; }
    public int? Quantity { get; }

    public NPCShopTemplateItem(int order, IDataProperty property)
    {
        Order = order;
        
        TemplateID = property.Resolve<int>("item") ?? 0;

        Price = property.Resolve<int>("price");
        DiscountRate = property.Resolve<byte>("discountRate");

        TokenTemplateID = property.Resolve<int>("token");
        TokenPrice = property.Resolve<int>("tokenPrice");

        ItemPeriod = property.Resolve<int>("period");
        LevelLimited = property.Resolve<int>("levelLimit");
        UnitPrice = property.Resolve<double>("unitPrice");
        MaxPerSlot = property.Resolve<short>("maxPerSlot");
        Quantity = property.Resolve<short>("quantity");
    }
}
