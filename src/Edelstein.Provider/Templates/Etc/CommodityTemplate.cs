using System.Linq;
using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Etc
{
    public class CommodityTemplate : ITemplate
    {
        public int ID { get; set; }

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

        public static CommodityTemplate Parse(int id, IDataProperty property)
        {
            var t = new CommodityTemplate {ID = id};

            property.Resolve(p =>
            {
                t.ItemID = p.Resolve<int>("itemID") ?? 0;
                t.Count = p.Resolve<short>("count") ?? 1;
                t.Priority = p.Resolve<byte>("priority") ?? 0;
                t.Price = p.Resolve<int>("price") ?? 0;
                t.Bonus = p.Resolve<byte>("bonus") ?? 0;
                t.Period = p.Resolve<short>("period") ?? 0;
                t.ReqPOP = p.Resolve<short>("reqPOP") ?? 0;
                t.ReqLEV = p.Resolve<short>("reqLEV") ?? 0;
                t.MaplePoint = p.Resolve<int>("maplePoint") ?? 0;
                t.Meso = p.Resolve<int>("meso") ?? 0;
                t.ForPremiumUser = p.Resolve<bool>("premium") ?? false;
                t.Gender = p.Resolve<sbyte>("gender") ?? 0;
                t.OnSale = p.Resolve<bool>("onSale") ?? false;
                t.Class = p.Resolve<byte>("class") ?? 0;
                t.Limit = p.Resolve<byte>("limit") ?? 0;
                t.PbCash = p.Resolve<short>("pbCash") ?? 0;
                t.PbPoint = p.Resolve<short>("pbPoint") ?? 0;
                t.PbGift = p.Resolve<short>("pbGift") ?? 0;
                t.PackageSN = p.Resolve("package")?.Children
                                  .Select(c => c.Resolve<int>() ?? 0)
                                  .ToArray() ?? null;
            });

            return t;
        }
    }
}