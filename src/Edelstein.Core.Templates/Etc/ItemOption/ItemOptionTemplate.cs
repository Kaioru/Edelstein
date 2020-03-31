using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Edelstein.Provider;

namespace Edelstein.Core.Templates.Etc.ItemOption
{
    public class ItemOptionTemplate : IDataTemplate
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
                .ToImmutableDictionary(
                    l => Convert.ToInt32(l.Name),
                    l => new ItemOptionLevelTemplate(l.ResolveAll())
                );
        }
    }
}