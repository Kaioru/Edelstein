using System.Collections.Generic;
using System.Linq;
using Edelstein.Provider.Parser;

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

        public static NPCTemplate Parse(int id, IDataProperty property)
        {
            var t = new NPCTemplate {ID = id};

            property.Resolve(p =>
            {
                p.Resolve("info").Resolve(i =>
                {
                    t.TrunkPut = i.Resolve<int>("trunkPut") ?? 0;
                    t.TrunkGet = i.Resolve<int>("trunkGet") ?? 0;
                    t.StoreBank = i.Resolve<bool>("storeBank") ?? false;
                    t.Parcel = i.Resolve<bool>("parcel") ?? false;
                    t.Scripts = i.Resolve("script")?.Children
                                    .Select(NPCScriptTemplate.Parse)
                                    .ToList()
                                ?? new List<NPCScriptTemplate>();
                });
                
                t.Move = p.Resolve("move") != null;
            });
            return t;
        }
    }
}