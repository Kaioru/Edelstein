using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Provider;

namespace Edelstein.Core.Templates.NPC
{
    public class NPCTemplate : IDataTemplate
    {
        public int ID { get; }

        public bool Move { get; }

        public int TrunkPut { get; private set; }
        public int TrunkGet { get; private set; }

        public bool Trunk => TrunkPut > 0 || TrunkGet > 0;
        public bool StoreBank { get; private set; }
        public bool Parcel { get; private set; }

        public ICollection<NPCScriptTemplate> Scripts;

        public NPCTemplate(int id, IDataProperty property)
        {
            ID = id;

            Move = property.Resolve("move") != null;

            property.Resolve("info").ResolveAll(i =>
            {
                TrunkPut = i.Resolve<int>("trunkPut") ?? 0;
                TrunkGet = i.Resolve<int>("trunkGet") ?? 0;
                StoreBank = i.Resolve<bool>("storeBank") ?? false;
                Parcel = i.Resolve<bool>("parcel") ?? false;
                Scripts = i.Resolve("script")?.Children
                              .Select(p => new NPCScriptTemplate(p))
                              .ToImmutableList()
                          ?? ImmutableList<NPCScriptTemplate>.Empty;
            });
        }
    }
}