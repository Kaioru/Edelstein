using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Item.Option
{
    public class ItemOptionTemplate : ITemplate
    {
        public int ID { get; set; }
        public short ReqLevel { get; set; }
        public int OptionType { get; set; }
        public IDictionary<int, ItemOptionLevelTemplate> LevelData;

        public static ItemOptionTemplate Parse(int id, IDataProperty property)
        {
            var t = new ItemOptionTemplate {ID = id};

            property.Resolve(p =>
            {
                p.Resolve("info")?.Resolve(i =>
                {
                    t.ReqLevel = i?.Resolve<short>("reqLevel") ?? 0;
                    t.OptionType = i?.Resolve<short>("optionType") ?? 0;
                });

                t.LevelData = p.Resolve("level").Children
                    .ToDictionary(
                        l => Convert.ToInt32(l.Name),
                        l => ItemOptionLevelTemplate.Parse(Convert.ToInt32(l.Name), l)
                    );
            });
            return t;
        }
    }
}