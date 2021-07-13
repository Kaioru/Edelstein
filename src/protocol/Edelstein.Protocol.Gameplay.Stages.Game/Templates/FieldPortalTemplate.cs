using Edelstein.Protocol.Gameplay.Space;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Templates
{
    public record FieldPortalTemplate : IPhysicalPoint2D, ITemplate
    {
        public int ID { get; init; }
        public FieldPortalType Type { get; init; }

        public string Name { get; init; }
        public string Script { get; init; }

        public int ToMap { get; init; }
        public string ToName { get; init; }

        public Point2D Position { get; init; }
    }
}
