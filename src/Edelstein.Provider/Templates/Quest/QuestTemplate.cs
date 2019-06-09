using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Provider.Logging;
using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Quest
{
    public class QuestTemplate : ITemplate
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public int ID { get; }

        public IDictionary<QuestState, QuestActTemplate> Act { get; }
        public IDictionary<QuestState, QuestCheckTemplate> Check { get; }

        public QuestTemplate(int id, IDataProperty info, IDataProperty act, IDataProperty check)
        {
            ID = id;

            Act = act.Children.ToImmutableDictionary(
                c => (QuestState) Convert.ToInt32(c.Name),
                c => new QuestActTemplate(Convert.ToInt32(c.Name), c.ResolveAll())
            );
            Check = check.Children.ToImmutableDictionary(
                c => (QuestState) Convert.ToInt32(c.Name),
                c => new QuestCheckTemplate(Convert.ToInt32(c.Name), c.ResolveAll())
            );
        }
    }
}