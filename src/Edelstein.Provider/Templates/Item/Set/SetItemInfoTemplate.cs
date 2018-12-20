using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Provider.Parser;

namespace Edelstein.Provider.Templates.Item.Set
{
    public class SetItemInfoTemplate : ITemplate
    {
        public int ID { get; set; }
        public int SetCompleteCount { get; set; }

        public ICollection<int> ItemTemplateID { get; set; }
        public IDictionary<int, SetItemEffectTemplate> Effect { get; set; }

        public static SetItemInfoTemplate Parse(int id, IDataProperty property)
        {
            var t = new SetItemInfoTemplate {ID = id};

            property.Resolve(p =>
            {
                t.SetCompleteCount = p.Resolve<int>("completeCount") ?? 0;

                t.ItemTemplateID = p.Resolve("itemID")?.Children
                    .Select(c => c.Resolve<int>() ?? 0)
                    .ToList();
                t.Effect = p.Resolve("Effect")?.Children
                    .ToDictionary(
                        c => Convert.ToInt32(c.Name),
                        c => SetItemEffectTemplate.Parse(Convert.ToInt32(c.Name), c)
                    );
            });
            return t;
        }
    }
}