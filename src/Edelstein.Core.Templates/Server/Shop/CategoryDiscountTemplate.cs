using System;
using Edelstein.Provider;

namespace Edelstein.Core.Templates.Server.Shop
{
    public class CategoryDiscountTemplate : IDataTemplate
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