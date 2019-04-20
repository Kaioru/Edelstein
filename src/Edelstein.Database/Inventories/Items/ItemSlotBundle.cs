namespace Edelstein.Database.Inventories.Items
{
    public class ItemSlotBundle : ItemSlot
    {
        public short Number { get; set; }
        public short MaxNumber { get; set; }
        public short Attribute { get; set; }

        public string Title { get; set; }
    }
}