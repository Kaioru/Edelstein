using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Etc.NPCShop
{
    public class NPCShopTemplate : ITemplate
    {
        public int ID { get; }
        public IDictionary<int, NPCShopItemTemplate> Items { get; }

        public NPCShopTemplate(int id, IDataProperty property)
        {
            ID = id;
            Items = property.Children
                .ToImmutableDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => new NPCShopItemTemplate(Convert.ToInt32(c.Name), c.ResolveAll())
                );
        }
    }
}