using System.Collections.Generic;
using System.Linq;

namespace Edelstein.Provider.Templates.NPC
{
    public class NPCTemplate : ITemplate
    {
        public int ID { get; set; }

        public int TrunkPut { get; set; }
        public int TrunkGet { set; get; }

        public bool Trunk => TrunkPut > 0 || TrunkGet > 0;
        public bool StoreBank { get; set; }
        public bool Parcel { get; set; }
        public ICollection<NPCScriptTemplate> Scripts;

        public bool Move { get; set; }

        public NPCTemplate(int id, IDataProperty property)
        {
            ID = id;
 
            property.Resolve("info").ResolveAll(i =>
            {
                TrunkPut = i.Resolve<int>("trunkPut") ?? 0;
                TrunkGet = i.Resolve<int>("trunkGet") ?? 0;
                StoreBank = i.Resolve<bool>("storeBank") ?? false;
                Parcel = i.Resolve<bool>("parcel") ?? false;
                Scripts = i.Resolve("script")?.Children
                              .Select(p => new NPCScriptTemplate(p))
                              .ToList()
                          ?? new List<NPCScriptTemplate>();
            });

            Move = property.Resolve("move") != null;
        }
    }
}