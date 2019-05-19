using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Provider.Logging;

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

            Act = act.Children.ToDictionary(
                c => (QuestState) Convert.ToInt32(c.Name),
                c => new QuestActTemplate(Convert.ToInt32(c.Name), c.ResolveAll())
            );
            Check = act.Children.ToDictionary(
                c => (QuestState) Convert.ToInt32(c.Name),
                c => new QuestCheckTemplate(Convert.ToInt32(c.Name), c.ResolveAll())
            );
        }
    }
}