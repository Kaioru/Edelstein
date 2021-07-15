using Edelstein.Protocol.Gameplay.Spatial;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Templates
{
    public record FieldFootholdTemplate : IPhysicalLine2D, ITemplate
    {
        public int ID { get; init; }

        public int Next { get; init; }
        public int Prev { get; init; }

        public Line2D Line { get; init; }
    }
}
