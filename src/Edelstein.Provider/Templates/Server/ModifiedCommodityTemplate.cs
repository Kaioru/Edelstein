using System.Linq;

namespace Edelstein.Provider.Templates.Server
{
    public class ModifiedCommodityTemplate : ITemplate
    {
        public int ID { get; }

        public int? ItemID { get; set; }
        public short? Count { get; set; }
        public byte? Priority { get; set; }
        public int? Price { get; set; }
        public byte? Bonus { get; set; }
        public short? Period { get; set; }
        public short? ReqPOP { get; set; }
        public short? ReqLEV { get; set; }
        public int? MaplePoint { get; set; }
        public int? Meso { get; set; }
        public bool? ForPremiumUser { get; set; }
        public byte? Gender { get; set; }
        public bool? OnSale { get; set; }
        public byte? Class { get; set; }
        public byte? Limit { get; set; }
        public short? PbCash { get; set; }
        public short? PbPoint { get; set; }
        public short? PbGift { get; set; }
        public int[] PackageSN { get; set; }

        public ModifiedCommodityTemplate(int id, IDataProperty property)
        {
            ID = id;

            ItemID = property.Resolve<int>("itemId");
            Count = property.Resolve<short>("count");
            Priority = property.Resolve<byte>("priority");
            Price = property.Resolve<int>("price");
            Bonus = property.Resolve<byte>("bonus");
            Period = property.Resolve<short>("period");
            ReqPOP = property.Resolve<short>("reqPOP");
            ReqLEV = property.Resolve<short>("reqLEV");
            MaplePoint = property.Resolve<int>("maplePoint");
            Meso = property.Resolve<int>("meso");
            ForPremiumUser = property.Resolve<bool>("premium");
            Gender = property.Resolve<byte>("gender");
            OnSale = property.Resolve<bool>("onSale");
            Class = property.Resolve<byte>("class");
            Limit = property.Resolve<byte>("limit");
            PbCash = property.Resolve<short>("pbCash");
            PbPoint = property.Resolve<short>("pbPoint");
            PbGift = property.Resolve<short>("pbGift");
            PackageSN = property.Resolve("package")?.Children
                .Select(c => c.Resolve<int>() ?? 0)
                .ToArray();
        }
    }
}