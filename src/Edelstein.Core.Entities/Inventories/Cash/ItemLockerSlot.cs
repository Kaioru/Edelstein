using System;
using Edelstein.Core.Entities.Inventories.Items;

namespace Edelstein.Core.Entities.Inventories.Cash
{
    public class ItemLockerSlot
    {
        public ItemSlot Item { get; set; }

        public int AccountID { get; set; }
        public int CharacterID { get; set; }
        public int CommodityID { get; set; }
        public string BuyCharacterName { get; set; }
        public int PaybackRate { get; set; }
        public int DiscountRate { get; set; }

        public ItemLockerSlot()
        {
        }

        public ItemLockerSlot(ItemSlot item)
        {
            Item = item;
            Item.CashItemSN ??= DateTime.Now.Ticks;
        }
    }
}