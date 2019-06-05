using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Edelstein.Provider.Templates.Item.Consume
{
    public class MobSummonItemTemplate : ItemBundleTemplate
    {
        public ICollection<MobSummonItemEntry> Mobs { get; set; }

        public MobSummonItemTemplate(int id, IDataProperty info, IDataProperty mob) : base(id, info)
        {
            Mobs = mob.Children
                .Select(c => new MobSummonItemEntry(c.ResolveAll()))
                .ToList();
        }
    }

    public class MobSummonItemEntry
    {
        public int TemplateID { get; set; }
        public int Prob { get; set; }

        public MobSummonItemEntry(IDataProperty property)
        {
            TemplateID = property.Resolve<int>("id") ?? 0;
            Prob = property.Resolve<int>("prob") ?? 0;
        }
    }
}