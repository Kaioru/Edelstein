using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Dialogs.Shop.Templates;

namespace Edelstein.Common.Gameplay.Game.Dialogs.Shop.Templates;

public record ShopTemplateItem : IShopTemplateItem
{
    public int ItemID { get; }
    
    public int Price { get; }
    public byte DiscountRate { get; }
    
    public int TokenItemID { get; }
    public int TokenPrice { get; }
    
    public int ItemPeriod { get; }
    public int LevelLimited { get; }
    
    public short Quantity { get; }
    
    public double UnitPrice { get; }
    public short MaxPerSlot { get; }
    
    public int Stock { get; }

    public ShopTemplateItem(IDataNode node)
    {
        ItemID = node.ResolveInt("item") ?? 0;
        
        Price = node.ResolveInt("price") ?? 0;
        DiscountRate = node.ResolveByte("discountRate") ?? 0;

        TokenItemID = node.ResolveInt("token") ?? 0;
        TokenPrice = node.ResolveInt("tokenPrice") ?? 0;

        ItemPeriod = node.ResolveInt("period") ?? 0;
        LevelLimited = node.ResolveInt("levelLimit") ?? 0;
        
        Quantity = node.ResolveShort("quantity") ?? 1;
        
        UnitPrice = node.ResolveDouble("unitPrice") ?? 0.0;
        MaxPerSlot = node.ResolveShort("maxPerSlot") ?? 100;
        
        Stock = node.ResolveShort("stock") ?? 1;
    }
}
