using System;

namespace Edelstein.Provider.Templates.Shop
{
    public class BestTemplate : ITemplate
    {
        public int ID { get; }

        public int Category { get; }
        public int Gender { get; }
        public int CommoditySN { get; }

        public BestTemplate(int id, IDataProperty property)
        {
            ID = id;

            Category = Convert.ToInt32(property.Parent.Parent.Name);
            Gender = property.Parent.Name == "male" ? 0 : 1;
            CommoditySN = property.Resolve<int>() ?? 0;
        }
    }
}