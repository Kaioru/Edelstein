using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Util.Spatial;

public readonly struct Segment2D : ISegment2D
{
    public float MinX => Math.Min(P1.X, P2.X);
    public float MinY => Math.Min(P1.Y, P2.Y);
    public float MaxX => Math.Max(P1.X, P2.X);
    public float MaxY => Math.Max(P1.Y, P2.Y);

    public IPoint2D P1 { get; }
    public IPoint2D P2 { get; }

    public Segment2D(IPoint2D p1, IPoint2D p2)
    {
        P1 = p1;
        P2 = p2;
    }

    public IPoint2D Middle => new Point2D((P1.X + P2.X) / 2, (P1.Y + P2.Y) / 2);

    public float Length => P1.Distance(P2);
    public float? Slope => IsVertical ? null : (P2.Y - P1.Y) / (P2.X - P1.X);

    public bool IsVertical => Math.Abs(P1.X - P2.X) <= 0;
    public bool IsHorizontal => Math.Abs(P1.Y - P2.Y) <= 0;

    public bool IsAbove(IPoint2D point)
        => Cross(point) > 0;

    public bool IsBelow(IPoint2D point)
        => Cross(point) < 0;

    public IPoint2D? AtX(float x)
        => x >= MinX && x <= MaxX
            ? Slope.HasValue
                ? new Point2D(x, (Slope.Value * (x - MinX)) + MinY)
                : null
            : null;

    public IPoint2D? AtY(float y)
        => y >= MinY && y <= MaxY
            ? Slope.HasValue
                ? new Point2D(((y - MinY) / Slope.Value) + MinX, y)
                : null
            : null;

    public bool Intersects(IPoint2D point)
        => Math.Abs(point.Distance(P1) + point.Distance(P2) - P1.Distance(P2)) < 0.01;

    private float Cross(IPoint2D point)
        => (point.X - P1.X) * (P2.Y - P1.Y) - (point.Y - P1.Y) * (P2.X - P1.X);
}
