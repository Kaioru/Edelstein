using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Parser;

namespace Edelstein.Protocol.Gameplay.Users.Inventories.Templates.Sets
{
    public record ItemSetTemplate : ITemplate
    {
        public int ID { get; }
        public int SetCompleteCount { get; }

        public ICollection<int> ItemID { get; }
        public IDictionary<int, ItemSetEffectTemplate> Effect { get; }

        public ItemSetTemplate(int id, IDataProperty property, IDataProperty itemID, IDataProperty effect)
        {
            ID = id;

            SetCompleteCount = property.Resolve<int>("completeCount") ?? 0;

            ItemID = itemID.Children
                .Select(c => c.Resolve<int>() ?? 0)
                .ToImmutableList();
            Effect = effect.Children
                .ToImmutableDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => new ItemSetEffectTemplate(Convert.ToInt32(c.Name), c.ResolveAll())
                );
        }

    }
}
