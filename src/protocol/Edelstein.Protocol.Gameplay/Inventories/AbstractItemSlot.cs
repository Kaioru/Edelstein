using System;

namespace Edelstein.Protocol.Gameplay.Inventories
{
    public abstract class AbstractItemSlot
    {
        public int TemplateID { get; set; }
        public DateTime? DateExpire { get; set; }
    }
}
