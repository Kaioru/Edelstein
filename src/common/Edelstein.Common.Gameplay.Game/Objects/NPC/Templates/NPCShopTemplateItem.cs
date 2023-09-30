using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;

public record NPCShopTemplateItem : INPCShopItem
{
    public int Order { get; }
    
    public int TemplateID { get; }
    
    public int Price { get; }
    public byte DiscountRate { get; }
    
    public int TokenTemplateID { get; }
    public int TokenPrice { get; }
    
    public int ItemPeriod { get; }
    public int LevelLimited { get; }
    
    public double UnitPrice { get; }
    public short MaxPerSlot { get; }
    public int Quantity { get; }

    public NPCShopTemplateItem(int order, IDataProperty property)
    {
        Order = order;
        
        TemplateID = property.Resolve<int>("item") ?? 0;

        Price = property.Resolve<int>("price") ?? 0;
        DiscountRate = property.Resolve<byte>("discountRate") ?? 0;

        TokenTemplateID = property.Resolve<int>("token") ?? 0;
        TokenPrice = property.Resolve<int>("tokenPrice") ?? 0;

        ItemPeriod = property.Resolve<int>("period") ?? 0;
        LevelLimited = property.Resolve<int>("levelLimit") ?? 0;
        
        UnitPrice = property.Resolve<double>("unitPrice") ?? 0.0;
        MaxPerSlot = property.Resolve<short>("maxPerSlot") ?? 100;
        Quantity = property.Resolve<short>("quantity") ?? 1;
    }
}
