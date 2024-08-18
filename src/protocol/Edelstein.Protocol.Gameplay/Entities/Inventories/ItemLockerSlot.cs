using Edelstein.Protocol.Gameplay.Entities.Inventories.Items;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories;

public record ItemLockerSlot
{
    public int AccountID { get; set; }
    public int CharacterID { get; set; }
    public int CommodityID { get; set; }

    public string? BuyCharacterName { get; set; }

    public int PaybackRate { get; set; }
    public int DiscountRate { get; set; }

    public required ItemSlotBase Item { get; set; }
}
