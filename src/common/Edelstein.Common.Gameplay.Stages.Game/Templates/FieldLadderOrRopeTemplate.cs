using Edelstein.Protocol.Gameplay.Spatial;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Parser;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Templates
{
    public record FieldLadderOrRopeTemplate : IPhysicalLine2D, ITemplate
    {
        public int ID { get; }

        public Line2D Line { get; }

        public FieldLadderOrRopeTemplate(int id, IDataProperty property)
        {
            ID = id;

            Line = new Line2D(
                new Point2D(property.Resolve<int>("x") ?? 0, property.Resolve<int>("y1") ?? 0),
                new Point2D(property.Resolve<int>("x") ?? 0, property.Resolve<int>("y2") ?? 0)
            );
        }
    }
}
