using Duey.Abstractions;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Gameplay.Game.Spatial;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Spatial;

public record FieldFoothold : IFieldFoothold
{

    public FieldFoothold(int id, IDataNode node)
    {
        ID = id;

        NextID = node.ResolveInt("next") ?? 0;
        PrevID = node.ResolveInt("prev") ?? 0;

        Line = new Segment2D(
            new Point2D(node.ResolveInt("x1") ?? 0, node.ResolveInt("y1") ?? 0),
            new Point2D(node.ResolveInt("x2") ?? 0, node.ResolveInt("y2") ?? 0)
        );
    }
    public int ID { get; }

    public int MinX => Line.MinX;
    public int MinY => Line.MinY;

    public int MaxX => Line.MaxX;
    public int MaxY => Line.MaxY;

    public int NextID { get; }
    public int PrevID { get; }

    public ISegment2D Line { get; }
}
