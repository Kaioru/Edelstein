using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Provider;

namespace Edelstein.Core.Templates.Etc.SetItemInfo
{
    public class SetItemInfoTemplate : IDataTemplate
    {
        public int ID { get; }
        public int SetCompleteCount { get; }

        public ICollection<int> TemplateID { get; }
        public IDictionary<int, SetItemEffectTemplate> Effect { get; }

        public SetItemInfoTemplate(int id, IDataProperty property)
        {
            ID = id;

            SetCompleteCount = property.Resolve<int>("completeCount") ?? 0;

            TemplateID = property.Resolve("ItemID")?.Children
                .Select(c => c.Resolve<int>() ?? 0)
                .ToImmutableList();
            Effect = property.Resolve("Effect")?.Children
                .ToImmutableDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => new SetItemEffectTemplate(c.ResolveAll())
                );
        }
    }
}