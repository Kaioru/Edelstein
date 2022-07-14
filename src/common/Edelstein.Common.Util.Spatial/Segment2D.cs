using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Util.Spatial;

public readonly struct Segment2D : ISegment2D
{
    public int MinX => Math.Min(P1.X, P2.X);
    public int MinY => Math.Min(P1.Y, P2.Y);
    public int MaxX => Math.Max(P1.X, P2.X);
    public int MaxY => Math.Max(P1.Y, P2.Y);

    public IPoint2D P1 { get; }
    public IPoint2D P2 { get; }

    public Segment2D(IPoint2D p1, IPoint2D p2)
    {
        P1 = p1;
        P2 = p2;
    }

    public IPoint2D Middle => new Point2D((P1.X + P2.X) / 2, (P1.Y + P2.Y) / 2);

    public double Length => P1.Distance(P2);
    public double Slope => IsVertical ? 0 : (P2.Y - P1.Y) / (P2.X - P1.X);

    public bool IsVertical => P1.X == P2.X;
    public bool IsHorizontal => P1.Y == P2.Y;

    public bool IsAbove(IPoint2D point)
        => Cross(point) > 0;

    public bool IsBelow(IPoint2D point)
        => Cross(point) < 0;

    public bool Intersects(IPoint2D point)
        => Math.Abs(point.Distance(P1) + point.Distance(P2) - P1.Distance(P2)) < 0.01;

    private int Cross(IPoint2D point)
        => (point.X - P1.X) * (P2.Y - P1.Y) - (point.Y - P1.Y) * (P2.X - P1.X);
}
