using System;

namespace Edelstein.Protocol.Util.Spatial
{
    public struct Point2D
    {
        public int X { get; }
        public int Y { get; }

        public Point2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public double Distance(Point2D target)
        {
            int distX = target.X - X;
            int distY = target.Y - Y;

            return Math.Sqrt(distX * distX + distY * distY);
        }

        public override string ToString()
            => $"({X}, {Y})";
    }
}
