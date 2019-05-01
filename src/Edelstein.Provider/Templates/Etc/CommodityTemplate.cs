namespace Edelstein.Provider.Templates.Etc
{
    public class CommodityTemplate : ITemplate
    {
        public int ID { get; }

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