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

        public static ItemOptionTemplate Parse(int id, IDataProperty p)
        {
            var info = p.Resolve("info");
            
            return new ItemOptionTemplate
            {
                ID = id,
                ReqLevel = info?.Resolve<short>("reqLevel") ?? 0,
                OptionType = info?.Resolve<short>("optionType") ?? 0,
                LevelData = p.Resolve("level").Children
                    .ToDictionary(
                        c => Convert.ToInt32(c.Name),
                        c => ItemOptionLevelTemplate.Parse(Convert.ToInt32(p.Name), c)
                    )
            };
        }
    }
}