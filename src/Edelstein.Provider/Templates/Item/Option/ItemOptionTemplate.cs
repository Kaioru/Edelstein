using System;
using System.Collections.Generic;
using System.Linq;

namespace Edelstein.Provider.Templates.Item.Option
{
    public class ItemOptionTemplate : ITemplate
    {
        public int ID { get; }

        public short ReqLevel { get; private set; }
        public int OptionType { get; private set; }
        public IDictionary<int, ItemOptionLevelTemplate> LevelData { get; }

        public ItemOptionTemplate(int id, IDataProperty property)
        {
            ID = id;

            property.Resolve("info")?.ResolveAll(i =>
            {
                ReqLevel = i?.Resolve<short>("reqLevel") ?? 0;
                OptionType = i?.Resolve<short>("optionType") ?? 0;
            });

            LevelData = property.Resolve("level").Children
                .ToDictionary(
                    l => Convert.ToInt32(l.Name),
                    l => new ItemOptionLevelTemplate(Convert.ToInt32(l.Name), l.ResolveAll())
                );
        }
    }
}