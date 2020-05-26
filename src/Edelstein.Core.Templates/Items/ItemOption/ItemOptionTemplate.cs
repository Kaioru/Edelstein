using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Edelstein.Core.Provider;

namespace Edelstein.Core.Templates.Items.ItemOption
{
    public class ItemOptionTemplate : IDataTemplate
    {
        public int ID { get; }

        public ItemOptionGrade Grade { get; }
        public ItemOptionType Type { get; private set; }
        public short ReqLevel { get; private set; }
        public IDictionary<int, ItemOptionLevelTemplate> LevelData { get; }

        public ItemOptionTemplate(int id, IDataProperty property)
        {
            ID = id;

            Grade = (ItemOptionGrade) (id / 10000);

            property.Resolve("info")?.ResolveAll(i =>
            {
                Type = (ItemOptionType) (i?.Resolve<short>("optionType") ?? 0);
                ReqLevel = i?.Resolve<short>("reqLevel") ?? 0;
            });

            LevelData = property.Resolve("level").Children
                .ToImmutableDictionary(
                    l => Convert.ToInt32(l.Name),
                    l => new ItemOptionLevelTemplate(l.ResolveAll())
                );
        }
    }
}