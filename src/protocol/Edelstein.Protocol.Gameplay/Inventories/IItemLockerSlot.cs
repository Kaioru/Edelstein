using Edelstein.Protocol.Gameplay.Inventories.Items;

namespace Edelstein.Protocol.Gameplay.Inventories;

public interface IItemLockerSlot : IItemSlot
{
    int AccountID { get; set; }
    int CharacterID { get; set; }
    int CommodityID { get; set; }
    
    string BuyCharacterName { get; set; }

    int PaybackRate { get; set; }
    int DiscountRate { get; set; }

    IItemSlot Item { get; set; }
}
