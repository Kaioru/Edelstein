using System.Linq;
using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Server.ModifiedCommodity
{
    public class ModifiedCommodityTemplate : ITemplate
    {
        public int ID { get; set; }

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

        public static ModifiedCommodityTemplate Parse(int id, IDataProperty property)
        {
            var t = new ModifiedCommodityTemplate {ID = id};

            property.Resolve(p =>
            {
                t.ItemID = p.Resolve<int>("itemId");
                t.Count = p.Resolve<short>("count");
                t.Priority = p.Resolve<byte>("priority");
                t.Price = p.Resolve<int>("price");
                t.Bonus = p.Resolve<byte>("bonus");
                t.Period = p.Resolve<short>("period");
                t.ReqPOP = p.Resolve<short>("reqPOP");
                t.ReqLEV = p.Resolve<short>("reqLEV");
                t.MaplePoint = p.Resolve<int>("maplePoint");
                t.Meso = p.Resolve<int>("meso");
                t.ForPremiumUser = p.Resolve<bool>("premium");
                t.Gender = p.Resolve<byte>("gender");
                t.OnSale = p.Resolve<bool>("onSale");
                t.Class = p.Resolve<byte>("class");
                t.Limit = p.Resolve<byte>("limit");
                t.PbCash = p.Resolve<short>("pbCash");
                t.PbPoint = p.Resolve<short>("pbPoint");
                t.PbGift = p.Resolve<short>("pbGift");
                t.PackageSN = property.Resolve("package")?.Children
                                .Select(c => c.Resolve<int>() ?? 0)
                                .ToArray() ?? null;
            });

            return t;
        }
    }
}