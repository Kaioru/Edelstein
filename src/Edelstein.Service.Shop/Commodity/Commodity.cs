using System;
using Edelstein.Data.Entities.Inventory;

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

        public ItemLockerSlot ToSlot()
        {
            var slot = new ItemLockerSlot
            {
                SN = DateTime.Now.Ticks,
                ItemID = ItemID,
                CommoditySN = SN,
                Number = Count
            };
            return slot;
        }
    }
}