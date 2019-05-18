using System;
using System.Collections.Generic;
using System.Linq;

namespace Edelstein.Provider.Templates.Item.Set
{
    public class SetItemInfoTemplate : ITemplate
    {
        public int ID { get; }
        public int SetCompleteCount { get; set; }

        public ICollection<int> TemplateID { get; set; }
        public IDictionary<int, SetItemEffectTemplate> Effect { get; set; }

        public SetItemInfoTemplate(int id, IDataProperty property)
        {
            ID = id;

            SetCompleteCount = property.Resolve<int>("completeCount") ?? 0;

            TemplateID = property.Resolve("ItemID")?.Children
                .Select(c => c.Resolve<int>() ?? 0)
                .ToList();
            Effect = property.Resolve("Effect")?.Children
                .ToDictionary(
                    c => Convert.ToInt32(c.Name),
                    c => new SetItemEffectTemplate(Convert.ToInt32(c.Name), c.ResolveAll())
                );
        }
    }
}