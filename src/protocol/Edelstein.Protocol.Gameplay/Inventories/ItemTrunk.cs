namespace Edelstein.Protocol.Gameplay.Inventories
{
    public class ItemTrunk : ItemInventory
    {
        public int Money { get; set; }

        public ItemTrunk()
        {
        }

        public ItemTrunk(short slotMax) : base(slotMax)
        {
        }
    }
}
