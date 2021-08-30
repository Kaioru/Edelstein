using Edelstein.Protocol.Gameplay.Spatial;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Parser;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Templates
{
    public record FieldPortalTemplate : IPhysicalPoint2D, ITemplate
    {
        public int ID { get; }
        public FieldPortalType Type { get; }

        public string Name { get; }
        public string Script { get; }

        public int ToMap { get; }
        public string ToName { get; }

        public Point2D Position { get; }

        public FieldPortalTemplate(int id, IDataProperty property)
        {
            ID = id;

            Name = property.ResolveOrDefault<string>("pn");
            Type = (FieldPortalType)(property.Resolve<int>("pt") ?? 0);
            Script = property.ResolveOrDefault<string>("script");
            ToMap = property.Resolve<int>("tm") ?? int.MinValue;
            ToName = property.ResolveOrDefault<string>("tn");
            Position = new Point2D(
                property.Resolve<int>("x") ?? int.MinValue,
                property.Resolve<int>("y") ?? int.MinValue
            );
        }
    }
}
