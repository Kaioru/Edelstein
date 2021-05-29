using System;
namespace Edelstein.Protocol.Gameplay.Inventories
{
    public class ItemSlotBundle : AbstractItemSlot
    {
        public short Number { get; set; }
        public short Attribute { get; set; }

        public string Title { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ItemSlotBundle b &&
                   TemplateID == b.TemplateID &&
                   Attribute == b.Attribute &&
                   Title == b.Title &&
                   DateExpire == b.DateExpire;
        }

        public override int GetHashCode() => HashCode.Combine(TemplateID, Attribute, Title, DateExpire);
    }
}
