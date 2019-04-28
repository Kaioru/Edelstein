namespace Edelstein.Database.Entities.Inventories
{
    public class ItemTrunk : ItemInventory
    {
        public int Money { get; set; }

        public ItemTrunk(short slotMax) : base(slotMax)
        {
        }
    }
}