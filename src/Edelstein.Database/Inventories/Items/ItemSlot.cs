using System;

namespace Edelstein.Database.Inventories.Items
{
    public abstract class ItemSlot
    {
        public int TemplateID { get; set; }
        public long? CashItemSN { get; set; }
        public DateTime? DateExpire { get; set; }
    }
}