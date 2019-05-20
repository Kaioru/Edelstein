namespace Edelstein.Provider.Templates.Etc.NPCShop
{
    public class NPCShopItemTemplate : ITemplate
    {
        public int ID { get; set; }
        
        public int TemplateID { get; set; }
        public int Price { get; set; }
        public byte DiscountRate { get; set; }
        public int TokenTemplateID { get; set; }
        public int TokenPrice { get; set; }
        public int ItemPeriod { get; set; }
        public int LevelLimited { get; set; }
        public double UnitPrice { get; set; }
        public short MaxPerSlot { get; set; }
        public int Stock { get; set; }
        public short Quantity { get; set; }

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