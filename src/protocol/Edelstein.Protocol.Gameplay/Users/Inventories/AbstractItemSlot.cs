using System;

namespace Edelstein.Protocol.Gameplay.Users.Inventories
{
    public abstract class AbstractItemSlot
    {
        public int TemplateID { get; set; }

        public long? CashItemSN { get; set; }
        public DateTime? DateExpire { get; set; }
    }
}
