using System;
namespace Edelstein.Protocol.Gameplay.Users.Inventories
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
                   CashItemSN == b.CashItemSN &&
                   Attribute == b.Attribute &&
                   Title == b.Title &&
                   DateExpire == b.DateExpire;
        }

        public override int GetHashCode() => HashCode.Combine(TemplateID, CashItemSN, Attribute, Title, DateExpire);
    }
}
