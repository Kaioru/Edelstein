using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Provider.Parser;
using MoreLinq.Extensions;

namespace Edelstein.Provider.Templates.Server.NPCShop
{
    public class NPCShopTemplate : ITemplate
    {
        public int ID { get; set; }

        public IDictionary<int, NPCShopItemTemplate> Items { get; set; }

        public static NPCShopTemplate Parse(int id, IDataProperty property)
        {
            var t = new NPCShopTemplate
            {
                ID = id,
                Items = property.Children
                    .Select(NPCShopItemTemplate.Parse)
                    .DistinctBy(x => x.ID)
                    .ToDictionary(x => x.ID, x => x)
            };

            return t;
        }
    }
}