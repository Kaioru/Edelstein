using System;

namespace Edelstein.Provider.Templates.Server
{
    public class CategoryDiscountTemplate : ITemplate
    {
        public int ID { get; }
        
        public byte Category { get; set; }
        public byte CategorySub { get; set; }
        public byte DiscountRate { get; set; }

        public CategoryDiscountTemplate(int id, IDataProperty property)
        {
            ID = id;

            Category = Convert.ToByte(property.Parent.Name);
            CategorySub = Convert.ToByte(property.Name);
            DiscountRate = property.Resolve<byte>() ?? 0;
        }
    }
}