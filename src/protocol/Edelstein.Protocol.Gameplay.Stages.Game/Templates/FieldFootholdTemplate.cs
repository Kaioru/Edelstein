using Edelstein.Protocol.Gameplay.Spatial;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Parser;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Templates
{
    public record FieldFootholdTemplate : IPhysicalLine2D, ITemplate
    {
        public int ID { get; init; }

        public int Next { get; init; }
        public int Prev { get; init; }

        public Line2D Line { get; init; }

        public FieldFootholdTemplate(int id, IDataProperty property)
        {
            ID = id;

            Next = property.Resolve<int>("next") ?? 0;
            Prev = property.Resolve<int>("prev") ?? 0;

            Line = new Line2D(
                new Point2D(property.Resolve<int>("x1") ?? 0, property.Resolve<int>("y1") ?? 0),
                new Point2D(property.Resolve<int>("x2") ?? 0, property.Resolve<int>("y2") ?? 0)
            );
        }
    }
}
