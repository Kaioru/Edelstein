using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Parser;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Templates
{
    public record NPCTemplate : ITemplate, IFieldObjNPCInfo
    {
        public int ID { get; }

        public bool Move { get; }

        public int TrunkPut { get; }
        public int TrunkGet { get; }

        public bool IsTrunk => TrunkPut > 0 || TrunkGet > 0;
        public bool IsStoreBank { get; }
        public bool IsParcel { get; }

        public string Script => Scripts.FirstOrDefault()?.Script;

        public ICollection<NPCScriptTemplate> Scripts;

        public NPCTemplate(int id, IDataProperty property, IDataProperty info)
        {
            ID = id;

            Move = property.Resolve("move") != null;

            TrunkPut = info.Resolve<int>("trunkPut") ?? 0;
            TrunkGet = info.Resolve<int>("trunkGet") ?? 0;
            IsStoreBank = info.Resolve<bool>("storeBank") ?? false;
            IsParcel = info.Resolve<bool>("parcel") ?? false;
            Scripts = info.Resolve("script")?.Children
                          .Where(p => p.Name.All(char.IsDigit)) // 1057006 causes errors
                          .Select(p => new NPCScriptTemplate(Convert.ToInt32(p.Name), p))
                          .ToImmutableList()
                      ?? ImmutableList<NPCScriptTemplate>.Empty;
        }
    }
}
