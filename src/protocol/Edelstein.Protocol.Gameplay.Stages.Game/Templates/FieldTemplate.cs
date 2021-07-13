using System.Collections.Generic;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Templates
{
    public record FieldTemplate : ITemplate
    {
        public int ID { get; init; }

        public FieldOpt Limit { get; init; }
        public Rect2D Bounds { get; init; }

        public int? FieldReturn { get; init; }
        public int? ForcedReturn { get; init; }

        public string ScriptFirstUserEnter { get; init; }
        public string ScriptUserEnter { get; init; }

        public IDictionary<int, FieldFootholdTemplate> Footholds { get; }
        public IDictionary<int, FieldPortalTemplate> Portals { get; }
        //public ICollection<FieldLifeTemplate> Life { get; }
        //public ICollection<FieldReactorTemplate> Reactors { get; }

        public double MobRate { get; init; }
        public int MobCapacityMin { get; init; }
        public int MobCapacityMax { get; init; }
    }
}
