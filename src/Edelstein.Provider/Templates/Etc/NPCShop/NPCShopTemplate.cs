using System;
using System.Collections.Generic;
using System.Linq;

namespace Edelstein.Provider.Templates.Etc.NPCShop
{
    public class NPCShopTemplate : ITemplate
    {
        public int ID { get; set; }
        public IDictionary<int, NPCShopItemTemplate> Items { get; set; }

        public NPCShopTemplate(int id, IDataProperty property)
        {
            ID = id;
            Items = property.Children
                .ToDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => new NPCShopItemTemplate(Convert.ToInt32(c.Name), c.ResolveAll())
                );
        }
    }
}