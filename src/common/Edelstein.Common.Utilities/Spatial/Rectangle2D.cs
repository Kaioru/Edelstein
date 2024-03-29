﻿using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Utilities.Spatial;

public readonly record struct Rectangle2D : IRectangle2D
{
    public int MinX => Left;
    public int MinY => Top;
    public int MaxX => Right;
    public int MaxY => Bottom;

    public IPoint2D P1 { get; }
    public IPoint2D P2 { get; }

    public IPoint2D Center => new Point2D((int)(MinX + Width / 2), (int)(MinY + Height / 2));

    public Rectangle2D(IPoint2D p1, IPoint2D p2)
    {
        P1 = p1;
        P2 = p2;
    }

    public Rectangle2D(IPoint2D center, IRectangle2D rect)
    {
        P1 = new Point2D((int)(center.X - rect.Width / 2), (int)(center.Y - rect.Height / 2));
        P2 = new Point2D((int)(center.X + rect.Width / 2), (int)(center.Y + rect.Height / 2));
    }

    public int Left => Math.Min(P1.X, P2.X);
    public int Right => Math.Max(P1.X, P2.X);
    public int Top => Math.Min(P1.Y, P2.Y);
    public int Bottom => Math.Max(P1.Y, P2.Y);

    public float Height => Math.Abs(P1.Y - P2.Y);
    public float Width => Math.Abs(P1.X - P2.X);
    public float Area => Height * Width;

    public bool IsSquare => Math.Abs(Height - Width) <= 0;

    public bool Intersects(IPoint2D point) =>
        point.X >= Left &&
        point.X <= Right &&
        point.Y >= Top &&
        point.Y <= Bottom;
}
