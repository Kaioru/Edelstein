namespace Edelstein.Protocol.Gameplay.Game.Dialogs.Shop.Templates;

public interface IShopTemplateItem
{
    int ItemID { get; }
    
    int Price { get; }
    byte DiscountRate { get; }
    
    int TokenItemID { get; }
    int TokenPrice { get; }
    
    int ItemPeriod { get; }
    int LevelLimited { get; }
    
    short Quantity { get; }
    
    double UnitPrice { get; }
    short MaxPerSlot { get; }
}
