using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Quest
{
    public class QuestOperationTemplate : ITemplate
    {
        public int ID { get; }

        public ICollection<QuestItemEntry> Items { get; }

        public QuestOperationTemplate(int id, IDataProperty property)
        {
            ID = id;

            Items = property.Resolve("item")?.Children
                        .Select(c => new QuestItemEntry(c.ResolveAll()))
                        .ToImmutableList()
                    ?? ImmutableList<QuestItemEntry>.Empty;
        }
    }

    public class QuestItemEntry
    {
        public int TemplateID { get; }
        public int Quantity { get; }

        public QuestItemEntry(IDataProperty property)
        {
            TemplateID = property.Resolve<int>("id") ?? 0;
            Quantity = property.Resolve<int>("count") ?? 1;
        }
    }
}