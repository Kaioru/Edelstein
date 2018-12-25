using System;
using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Server.NPCShop
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

        public static NPCShopItemTemplate Parse(IDataProperty property)
        {
            var t = new NPCShopItemTemplate {ID = Convert.ToInt32(property.Name)};

            property.Resolve(p =>
            {
                t.TemplateID = p.Resolve<int>("item") ?? 0;
                t.Price = p.Resolve<int>("price") ?? 0;
                t.DiscountRate = p.Resolve<byte>("discountRate") ?? 0;
                t.TokenTemplateID = p.Resolve<int>("token") ?? 0;
                t.TokenPrice = p.Resolve<int>("tokenPrice") ?? 0;
                t.ItemPeriod = p.Resolve<int>("period") ?? 0;
                t.LevelLimited = p.Resolve<int>("levelLimit") ?? 0;
                t.UnitPrice = p.Resolve<double>("unitPrice") ?? 0.0;
                t.MaxPerSlot = p.Resolve<short>("maxPerSlot") ?? 100;
                t.Stock = p.Resolve<int>("stock") ?? 1;
                t.Quantity = p.Resolve<short>("quantity") ?? 1;
            });
            return t;
        }
    }
}