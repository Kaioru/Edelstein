using System;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Utilities.Spatial;

public readonly record struct Point2D(
    int X, 
    int Y
) : IPoint2D
{
    public int MinX => X;
    public int MinY => Y;
    public int MaxX => X;
    public int MaxY => Y;

    public float Distance(IPoint2D point) 
        => MathF.Sqrt(MathF.Pow(X - point.X, 2) + MathF.Pow(Y - point.Y, 2));
}
