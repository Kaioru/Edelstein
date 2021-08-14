using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Parser;
using MoreLinq;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates
{
    public record NPCTemplate : ITemplate
    {
        public int ID { get; init; }

        public bool Move { get; init; }

        public int TrunkPut { get; init; }
        public int TrunkGet { get; init; }

        public bool Trunk => TrunkPut > 0 || TrunkGet > 0;
        public bool StoreBank { get; init; }
        public bool Parcel { get; init; }

        public ICollection<NPCScriptTemplate> Scripts;

        public NPCTemplate(int id, IDataProperty property, IDataProperty info)
        {
            ID = id;

            Move = property.Resolve("move") != null;

            TrunkPut = info.Resolve<int>("trunkPut") ?? 0;
            TrunkGet = info.Resolve<int>("trunkGet") ?? 0;
            StoreBank = info.Resolve<bool>("storeBank") ?? false;
            Parcel = info.Resolve<bool>("parcel") ?? false;
            Scripts = info.Resolve("script")?.Children
                          .Where(p => p.Name.All(char.IsDigit)) // 1057006 causes errors
                          .Select(p => new NPCScriptTemplate(Convert.ToInt32(p.Name), p))
                          .ToImmutableList()
                      ?? ImmutableList<NPCScriptTemplate>.Empty;
        }
    }
}
