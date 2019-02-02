using System;
using Edelstein.Core.Extensions;
using Edelstein.Data.Entities.Inventory;
using Edelstein.Provider.Templates.Item;

namespace Edelstein.Service.Shop.Commodity
{
    public class Commodity
    {
        public int SN { get; set; }
        public int ItemID { get; set; }
        public short Count { get; set; }
        public byte Priority { get; set; }
        public int Price { get; set; }
        public byte Bonus { get; set; }
        public short Period { get; set; }
        public short ReqPOP { get; set; }
        public short ReqLEV { get; set; }
        public int MaplePoint { get; set; }
        public int Meso { get; set; }
        public bool ForPremiumUser { get; set; }
        public sbyte Gender { get; set; }
        public bool OnSale { get; set; }
        public byte Class { get; set; }
        public byte Limit { get; set; }
        public short PbCash { get; set; }
        public short PbPoint { get; set; }
        public short PbGift { get; set; }
        public int[] PackageSN { get; set; }

        public ItemSlot ToItemSlot(ItemTemplate template)
        {
            var item = template.ToItemSlot();
            var now = DateTime.Now;
            
            item.CashItemSN = now.Ticks;

            if (Period > 0)
                if (item is ItemSlotPet pet)
                    pet.DateDead = now.AddDays(Period);
                else
                    item.DateExpire = now.AddDays(Period);

            return item;
        }
    }
}