using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Util.Spatial;

public struct Point2D : IPoint2D
{
    public int MinX => X;
    public int MinY => Y;
    public int MaxX => X;
    public int MaxY => Y;

    public int X { get; }
    public int Y { get; }

    public Point2D(int x, int y)
    {
        X = x;
        Y = y;
    }

    public double Distance(IPoint2D point)
        => Math.Sqrt(Math.Pow(X - point.X, 2) + Math.Pow(Y - point.Y, 2));
}
