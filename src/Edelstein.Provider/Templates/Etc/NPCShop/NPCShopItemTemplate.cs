namespace Edelstein.Provider.Templates.Etc.NPCShop
{
    public class NPCShopItemTemplate : ITemplate
    {
        public int ID { get; }

        public int TemplateID { get; }
        public int Price { get; }
        public byte DiscountRate { get; }
        public int TokenTemplateID { get; }
        public int TokenPrice { get; }
        public int ItemPeriod { get; }
        public int LevelLimited { get; }
        public double UnitPrice { get; }
        public short MaxPerSlot { get; }
        public int Stock { get; }
        public short Quantity { get; }

        public NPCShopItemTemplate(int id, IDataProperty property)
        {
            ID = id;

            TemplateID = property.Resolve<int>("item") ?? 0;
            Price = property.Resolve<int>("price") ?? 0;
            DiscountRate = property.Resolve<byte>("discountRate") ?? 0;
            TokenTemplateID = property.Resolve<int>("token") ?? 0;
            TokenPrice = property.Resolve<int>("tokenPrice") ?? 0;
            ItemPeriod = property.Resolve<int>("period") ?? 0;
            LevelLimited = property.Resolve<int>("levelLimit") ?? 0;
            UnitPrice = property.Resolve<double>("unitPrice") ?? 0.0;
            MaxPerSlot = property.Resolve<short>("maxPerSlot") ?? 100;
            Stock = property.Resolve<int>("stock") ?? 1;
            Quantity = property.Resolve<short>("quantity") ?? 1;
        }
    }
}