using Duey.Abstractions;
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

    public NPCShopTemplateItem(int order, IDataNode node)
    {
        Order = order;
        
        TemplateID = node.ResolveInt("item") ?? 0;

        Price = node.ResolveInt("price") ?? 0;
        DiscountRate = node.ResolveByte("discountRate") ?? 0;

        TokenTemplateID = node.ResolveInt("token") ?? 0;
        TokenPrice = node.ResolveInt("tokenPrice") ?? 0;

        ItemPeriod = node.ResolveInt("period") ?? 0;
        LevelLimited = node.ResolveInt("levelLimit") ?? 0;
        
        UnitPrice = node.ResolveDouble("unitPrice") ?? 0.0;
        MaxPerSlot = node.ResolveShort("maxPerSlot") ?? 100;
        Quantity = node.ResolveShort("quantity") ?? 1;
    }
}
