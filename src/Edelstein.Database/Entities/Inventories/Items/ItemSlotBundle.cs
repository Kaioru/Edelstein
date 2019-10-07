using System;

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
                   TemplateID == b.TemplateID &&
                   MaxNumber == b.MaxNumber &&
                   Attribute == b.Attribute &&
                   Title == b.Title &&
                   CashItemSN == b.CashItemSN &&
                   DateExpire == b.DateExpire;
        }

        public override int GetHashCode() => HashCode.Combine(TemplateID, MaxNumber, Attribute, Title, CashItemSN, DateExpire);
    }
}