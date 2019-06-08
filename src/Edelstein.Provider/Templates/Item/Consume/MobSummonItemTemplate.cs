using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Edelstein.Provider.Templates.Item.Consume
{
    public class MobSummonItemTemplate : ItemBundleTemplate
    {
        public ICollection<MobSummonItemEntry> Mobs { get; }

        public MobSummonItemTemplate(int id, IDataProperty info, IDataProperty mob) : base(id, info)
        {
            Mobs = mob.Children
                .Select(c => new MobSummonItemEntry(c.ResolveAll()))
                .ToImmutableList();
        }
    }

    public class MobSummonItemEntry
    {
        public int TemplateID { get; }
        public int Prob { get; }

        public MobSummonItemEntry(IDataProperty property)
        {
            TemplateID = property.Resolve<int>("id") ?? 0;
            Prob = property.Resolve<int>("prob") ?? 0;
        }
    }
}