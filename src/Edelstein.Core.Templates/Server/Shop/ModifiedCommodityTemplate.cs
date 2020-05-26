using System.Linq;
using Edelstein.Core.Provider;

namespace Edelstein.Core.Templates.Server.Shop
{
    public class ModifiedCommodityTemplate : IDataTemplate
    {
        public int ID { get; }

        public int? ItemID { get; }
        public short? Count { get; }
        public byte? Priority { get; }
        public int? Price { get; }
        public byte? Bonus { get; }
        public short? Period { get; }
        public short? ReqPOP { get; }
        public short? ReqLEV { get; }
        public int? MaplePoint { get; }
        public int? Meso { get; }
        public bool? ForPremiumUser { get; }
        public byte? Gender { get; }
        public bool? OnSale { get; }
        public byte? Class { get; }
        public byte? Limit { get; }
        public short? PbCash { get; }
        public short? PbPoint { get; }
        public short? PbGift { get; }
        public int[] PackageSN { get; }

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