using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Item.Reward
{
    public class RewardTemplate : ITemplate
    {
        public int ID { get; }

        public ICollection<RewardEntryTemplate> Entries { get; }

        public RewardTemplate(int id, IDataProperty property)
        {
            ID = id;

            Entries = property.Children
                .Select(c => new RewardEntryTemplate(
                    Convert.ToInt32(c.Name),
                    c.ResolveAll()))
                .ToImmutableList();
        }
    }
}