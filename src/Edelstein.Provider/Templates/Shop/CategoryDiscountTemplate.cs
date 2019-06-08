using System;
using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Shop
{
    public class CategoryDiscountTemplate : ITemplate
    {
        public int ID { get; }
        
        public byte Category { get; }
        public byte CategorySub { get; }
        public byte DiscountRate { get; }

        public CategoryDiscountTemplate(int id, IDataProperty property)
        {
            ID = id;

            Category = Convert.ToByte(property.Parent.Name);
            CategorySub = Convert.ToByte(property.Name);
            DiscountRate = property.Resolve<byte>() ?? 0;
        }
    }
}