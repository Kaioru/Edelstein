using System;
using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Server.CategoryDiscount
{
    public class CategoryDiscountTemplate : ITemplate
    {
        public int ID { get; set; }
        public byte Category { get; set; }
        public byte CategorySub { get; set; }
        public byte DiscountRate { get; set; }

        public static CategoryDiscountTemplate Parse(int id, IDataProperty property)
        {
            var t = new CategoryDiscountTemplate
            {
                ID = id,
                Category = Convert.ToByte(property.Parent.Name),
                CategorySub = Convert.ToByte(property.Name),
                DiscountRate = property.Resolve<byte>() ?? 0
            };

            return t;
        }
    }
}