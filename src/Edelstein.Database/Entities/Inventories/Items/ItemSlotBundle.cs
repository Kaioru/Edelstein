namespace Edelstein.Database.Entities.Inventories.Items
{
    public class ItemSlotBundle : ItemSlot
    {
        public short Number { get; set; }
        public short MaxNumber { get; set; }
        public short Attribute { get; set; }

        public string Title { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ItemSlotBundle b &&
                   Number == b.Number &&
                   MaxNumber == b.MaxNumber &&
                   Attribute == b.Attribute &&
                   Title == b.Title &&
                   CashItemSN == b.CashItemSN &&
                   DateExpire == b.DateExpire;
        }
    }
}