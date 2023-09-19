using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;

namespace Edelstein.Common.Gameplay.Models.Inventories;

public record ItemLockerSlot : IItemLockerSlot
{
    public int ID => Item.ID;

    public int AccountID { get; set; }
    public int CharacterID { get; set; }
    public int CommodityID { get; set; }
    public string? BuyCharacterName { get; set; }
    public int PaybackRate { get; set; }
    public int DiscountRate { get; set; }

    public IItemSlot Item { get; set; }
}
