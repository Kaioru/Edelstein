using System;
using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Server.Best
{
    public class BestTemplate : ITemplate
    {
        public int ID { get; set; }
        public int Category { get; set; }
        public int Gender { get; set; }
        public int CommoditySN { get; set; }

        public static BestTemplate Parse(int id, IDataProperty property)
        {
            var t = new BestTemplate
            {
                ID = id,
                Category = Convert.ToInt32(property.Parent.Parent.Name),
                Gender = property.Parent.Name == "male" ? 0 : 1,
                CommoditySN = property.Resolve<int>() ?? 0
            };

            return t;
        }
    }
}