using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Util.Spatial;

public readonly struct Point2D : IPoint2D
{
    public float MinX => X;
    public float MinY => Y;
    public float MaxX => X;
    public float MaxY => Y;

    public float X { get; }
    public float Y { get; }

    public Point2D(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point2D(float x, float y)
    {
        X = x;
        Y = y;
    }

    public float Distance(IPoint2D point)
        => MathF.Sqrt(MathF.Pow(X - point.X, 2) + MathF.Pow(Y - point.Y, 2));
}
