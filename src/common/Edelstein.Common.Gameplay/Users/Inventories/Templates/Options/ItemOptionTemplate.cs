using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Parser;

namespace Edelstein.Common.Gameplay.Users.Inventories.Templates.Options
{
    public record ItemOptionTemplate : ITemplate
    {
        public int ID { get; }

        public ItemOptionGrade Grade { get; }
        public ItemOptionType Type { get; }
        public short ReqLevel { get; }
        public IDictionary<int, ItemOptionLevelTemplate> LevelData { get; }

        public ItemOptionTemplate(int id, IDataProperty info, IDataProperty level)
        {
            ID = id;

            Grade = (ItemOptionGrade)(id / 10000);
            Type = (ItemOptionType)(info.Resolve<short>("optionType") ?? 0);
            ReqLevel = info.Resolve<short>("reqLevel") ?? 0;
            LevelData = level.Children
                .ToImmutableDictionary(
                    l => Convert.ToInt32(l.Name),
                    l => new ItemOptionLevelTemplate(Convert.ToInt32(l.Name), l.ResolveAll())
                );
        }
    }
}
