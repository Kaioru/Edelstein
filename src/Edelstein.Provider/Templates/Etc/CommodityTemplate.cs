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
                t.ItemID = p.Resolve<int>("ItemId") ?? 0;
                t.Count = p.Resolve<short>("Count") ?? 1;
                t.Priority = p.Resolve<byte>("Priority") ?? 0;
                t.Price = p.Resolve<int>("Price") ?? 0;
                t.Bonus = p.Resolve<byte>("Bonus") ?? 0;
                t.Period = p.Resolve<short>("Period") ?? 0;
                t.ReqPOP = p.Resolve<short>("ReqPOP") ?? 0;
                t.ReqLEV = p.Resolve<short>("ReqLEV") ?? 0;
                t.MaplePoint = p.Resolve<int>("MaplePoint") ?? 0;
                t.Meso = p.Resolve<int>("Meso") ?? 0;
                t.ForPremiumUser = p.Resolve<bool>("Premium") ?? false;
                t.Gender = p.Resolve<sbyte>("Gender") ?? 0;
                t.OnSale = p.Resolve<bool>("OnSale") ?? false;
                t.Class = p.Resolve<byte>("Class") ?? 0;
                t.Limit = p.Resolve<byte>("Limit") ?? 0;
                t.PbCash = p.Resolve<short>("PbCash") ?? 0;
                t.PbPoint = p.Resolve<short>("PbPoint") ?? 0;
                t.PbGift = p.Resolve<short>("PbGift") ?? 0;
                t.PackageSN = p.Resolve("Package")?.Children
                                  .Select(c => c.Resolve<int>() ?? 0)
                                  .ToArray() ?? null;
            });

            return t;
        }
    }
}