using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Spatial;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Spatial;

public record FieldFoothold : IFieldFoothold
{
    public int ID { get; }

    public int MinX => Line.MinX;
    public int MinY => Line.MinY;

    public int MaxX => Line.MaxX;
    public int MaxY => Line.MaxY;

    public int NextID { get; }
    public int PrevID { get; }

    public ISegment2D Line { get; }
    
    public FieldFoothold(int id, IDataProperty property)
    {
        ID = id;

        NextID = property.Resolve<int>("next") ?? 0;
        PrevID = property.Resolve<int>("prev") ?? 0;

        Line = new Segment2D(
            new Point2D(property.Resolve<int>("x1") ?? 0, property.Resolve<int>("y1") ?? 0),
            new Point2D(property.Resolve<int>("x2") ?? 0, property.Resolve<int>("y2") ?? 0)
        );
    }
}

