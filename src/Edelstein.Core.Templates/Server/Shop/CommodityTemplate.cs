using Edelstein.Core.Provider;

namespace Edelstein.Core.Templates.Server.Shop
{
    public class CommodityTemplate : IDataTemplate
    {
        public int ID { get; }

        public int ItemID { get; }
        public short Count { get; }
        public byte Priority { get; }
        public int Price { get; }
        public byte Bonus { get; }
        public short Period { get; }
        public short ReqPOP { get; }
        public short ReqLEV { get; }
        public int MaplePoint { get; }
        public int Meso { get; }
        public bool ForPremiumUser { get; }
        public sbyte Gender { get; }
        public bool OnSale { get; }
        public byte Class { get; }
        public byte Limit { get; }
        public short PbCash { get; }
        public short PbPoint { get; }
        public short PbGift { get; }

        public CommodityTemplate(int id, IDataProperty property)
        {
            ID = id;

            ItemID = property.Resolve<int>("ItemId") ?? 0;
            Count = property.Resolve<short>("Count") ?? 1;
            Priority = property.Resolve<byte>("Priority") ?? 0;
            Price = property.Resolve<int>("Price") ?? 0;
            Bonus = property.Resolve<byte>("Bonus") ?? 0;
            Period = property.Resolve<short>("Period") ?? 0;
            ReqPOP = property.Resolve<short>("ReqPOP") ?? 0;
            ReqLEV = property.Resolve<short>("ReqLEV") ?? 0;
            MaplePoint = property.Resolve<int>("MaplePoint") ?? 0;
            Meso = property.Resolve<int>("Meso") ?? 0;
            ForPremiumUser = property.Resolve<bool>("Premium") ?? false;
            Gender = property.Resolve<sbyte>("Gender") ?? 0;
            OnSale = property.Resolve<bool>("OnSale") ?? false;
            Class = property.Resolve<byte>("Class") ?? 0;
            Limit = property.Resolve<byte>("Limit") ?? 0;
            PbCash = property.Resolve<short>("PbCash") ?? 0;
            PbPoint = property.Resolve<short>("PbPoint") ?? 0;
            PbGift = property.Resolve<short>("PbGift") ?? 0;
        }
    }
}