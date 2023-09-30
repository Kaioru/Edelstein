namespace Edelstein.Protocol.Gameplay.Game.Objects.NPC;

public interface INPCShopItem
{
    int Order { get; }
    
    int TemplateID { get; }

    int? Price { get; }
    byte? DiscountRate { get; }

    int? TokenTemplateID { get; }
    int? TokenPrice { get; }

    int? ItemPeriod { get; }
    int? LevelLimited { get; }
    double? UnitPrice { get; }
    
    short? MaxPerSlot { get; }
    int? Quantity { get; }
}
