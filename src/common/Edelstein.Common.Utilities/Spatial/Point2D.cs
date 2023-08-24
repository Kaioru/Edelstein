using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Utilities.Spatial;

public readonly struct Point2D : IPoint2D
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

    public float Distance(IPoint2D point) => MathF.Sqrt(MathF.Pow(X - point.X, 2) + MathF.Pow(Y - point.Y, 2));
}
