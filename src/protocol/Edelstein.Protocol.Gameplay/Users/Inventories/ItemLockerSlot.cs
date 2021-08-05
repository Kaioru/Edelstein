namespace Edelstein.Protocol.Gameplay.Users.Inventories
{
    public class ItemLockerSlot
    {
        public AbstractItemSlot Item { get; set; }

        public int AccountID { get; set; }
        public int CharacterID { get; set; }
        public int CommodityID { get; set; }
        public string BuyCharacterName { get; set; }
        public int PaybackRate { get; set; }
        public int DiscountRate { get; set; }

        public ItemLockerSlot()
        {
        }

        public ItemLockerSlot(AbstractItemSlot item)
        {
            Item = item;
        }
    }
}
